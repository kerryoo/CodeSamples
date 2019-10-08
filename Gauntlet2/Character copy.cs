using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{ 
    protected const float INTERPOLATION = 10;
    protected const float BACKWARDS_RUN_SCALE = 0.66f;
    protected const float AIRSTRAFING_SCALE = 0.1f;
    protected const float MIN_JUMP_INTERVAL = 0.25f;

    protected MasterPlayer _masterPlayer;
    protected InputControl _inputControl;
    protected Transform _transform;
    protected CharacterState _currCharacterState;

    //fields for interpolating movement
    protected float _currentV;
    protected float _currentH;

    //fields for jumping logic
    protected bool _isGrounded;
    protected bool _jumped;
    protected bool _canDoubleJump;
    protected float _jumpTimeStamp;
    protected List<Collider> _collisions = new List<Collider>();

    //these fields are set during the loading of the game, unique to each character.
    protected CharacterStateLibrary _characterStateLibrary;
    protected Animator _animator;
    protected Rigidbody _rigidbody;
    protected CharacterStats _characterStats;
    protected GameObject _characterObject;

    //the fields we can't preset are the inputControl and transform, since they're unique
    //to each player. When a player gets the character, the inputControl and player's transform
    //will be assigned.
    public void prepareCharacter(MasterPlayer masterPlayer, InputControl inputControl, Transform passedTransform)
    {
        _masterPlayer = masterPlayer;
        _inputControl = inputControl;
        _transform = passedTransform;
    }

    public void setCharacterStats(CharacterStats characterStats)
    {
        _characterStats = characterStats;
    }

    //Some activities need to be handled even when the character is not active.
    //This method holds those activities to be passed to a multicast delegate
    //in the MasterPlayer class
    public abstract void passiveActivities();
    public abstract void addExperience();
    public abstract int getID();
    protected abstract void rankUp();
    protected abstract void basicAttack();
    protected abstract void specialMove();

    //Called in MasterPlayer when the player switches character.
    public virtual void setCharacterActive(bool active)
    {
        _characterObject.SetActive(active);
    }

    //inputs are handled through a finite state machine.
    //depending on which state _currCharacterState is pointing to, that state will
    //handle the input. Called in the MasterPlayer class's update loop.
    protected void handleInput()
    {
        _currCharacterState.handleInput();
        _jumped = false;
    }

    //called in the walking state to move the character.
    public void Move()
    {
        if (_inputControl.Direction.x < 0)
            _inputControl.Direction.x *= BACKWARDS_RUN_SCALE;

        TransformPosition();

        _animator.SetFloat("MoveSpeed", _inputControl.Direction.magnitude);
    }

    //called in the jumping state to strafe the character while in the air.
    public void AirStrafing()
    {
        _inputControl.Direction *= AIRSTRAFING_SCALE;
        TransformPosition();
    }

    //move the player (parent of the character).
    public void TransformPosition()
    {
        _currentH = Mathf.Lerp(_currentH, _inputControl.Direction.x, Time.deltaTime * INTERPOLATION);
        _currentV = Mathf.Lerp(_currentV, _inputControl.Direction.y, Time.deltaTime * INTERPOLATION);

        transform.position += _transform.forward * _currentV * _characterStats.MovementSpeed * Time.deltaTime +
            _transform.right * _currentH * _characterStats.MovementSpeed * Time.deltaTime;
    }


    //called from the character's walking, idle, and jumping state.
    //returns whether jump was registered to adjust state accordingly.
    public bool Jump() 
    {
        bool jumpCooldownOver = Time.time - _jumpTimeStamp >= MIN_JUMP_INTERVAL;

        if (!jumpCooldownOver || (!_isGrounded && !_canDoubleJump)) { return false; }

        if (jumpCooldownOver && _isGrounded)
        {
            _jumpTimeStamp = Time.time;
            _rigidbody.AddForce(Vector3.up * _characterStats.JumpForce, ForceMode.Impulse);
            _animator.SetBool("Grounded", false);
            _animator.SetTrigger("Jump");

            _jumped = true;
            _canDoubleJump = true;
            return true;
        }

        if (_canDoubleJump && !_isGrounded)
        {
            _jumped = true;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            _rigidbody.AddForce(Vector3.up * _characterStats.JumpForce, ForceMode.Impulse);
            _animator.SetTrigger("Jump");
            _canDoubleJump = false;
        }
        return true;
    }


    //called by enemy player's projectiles or abilities to apply a crowd control effect.
    public void crowdControl(int effect, float duration, params float[] intensity)
    {
        switch(effect)
        {
            case SwitchID.Stun:
                StartCoroutine(Stun(duration));
                break;
            case SwitchID.Slow:
                StartCoroutine(Slow(duration, intensity[0]));
                break;
            case SwitchID.Silence:
                StartCoroutine(Silence(duration));
                break;
            case SwitchID.Disarm:
                StartCoroutine(Disarm(duration));
                break;
        }
    }

    //IEnumerators to start coroutines for locking certain inputs.
    private IEnumerator Stun(float duration)
    {
        _inputControl.lockGameplayInput();
        yield return new WaitForSeconds(duration);
        _inputControl.unlockGameplayInput();
    }

    private IEnumerator Slow(float duration, float intensity)
    {
        float originalSpeed = _characterStats.MovementSpeed;
        _characterStats.MovementSpeed *= intensity;
        _animator.speed *= intensity;

        yield return new WaitForSeconds(duration);

        _characterStats.MovementSpeed = originalSpeed;
        _animator.speed = 1;
    }

    private IEnumerator Silence(float duration)
    {
        _inputControl.lockSpecial();
        yield return new WaitForSeconds(duration);
        _inputControl.unlockSpecial();
    }

    private IEnumerator Disarm(float duration)
    {
        _inputControl.lockAttack();
        yield return new WaitForSeconds(duration);
        _inputControl.unlockAttack();
    }


    //Collision methods for jumping mechanic.
    public void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            //check if the collision is beneath the character
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f) 
            {
                if (!_collisions.Contains(collision.collider))
                {
                    _collisions.Add(collision.collider);
                }
                _isGrounded = true;
            }
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            //check if the collision is beneath the character
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            _isGrounded = true;
            _animator.SetBool("Grounded", true);
            if (!_collisions.Contains(collision.collider))
                _collisions.Add(collision.collider);
        }
        else
        {
            if (_collisions.Contains(collision.collider))
                _collisions.Remove(collision.collider);
            
            if (_collisions.Count == 0)
                _isGrounded = false;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (_collisions.Contains(collision.collider))
            _collisions.Remove(collision.collider);
        
        if (_collisions.Count == 0)
            _isGrounded = false; 
    }

    public void implementPassiveItem(PassiveItem passiveItem)
    {
        _characterStats.MaxHealth += passiveItem.maxHealth;

        _characterStats.MaxEnergy += passiveItem.maxEnergy;
        _characterStats.EnergyRegen += passiveItem.energyRegen;
        _characterStats.ShootEnergyCost += passiveItem.shootEnergyCost;
        _characterStats.SpecialMovementCost += passiveItem.specialMovementCost;
        _characterStats.ShieldEnergyRate += passiveItem.shieldEnergyRate;

        _characterStats.RateOfFire /= passiveItem.rateOfFire; //rate of fire will always be handled by division;
        _characterStats.DamageDealt += passiveItem.damageDealt;
        _characterStats.MovementSpeed += passiveItem.movementSpeed;
    }

    public bool Equals(Character character)
    {
        return character.getID() == this.getID();
    }

}
