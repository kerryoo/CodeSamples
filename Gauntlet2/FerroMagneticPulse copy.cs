using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerroMagneticPulse : BasicPulse
{
    [SerializeField] float sightRadius;
    [SerializeField] float lifeTime;

    protected Rigidbody _rigidbody;
    protected MasterPlayer _playerLockOn;
    protected Vector3 _originalDirection;
    protected Vector3 _newDirection;
    
    public override void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _originalDirection = transform.forward;
        InvokeRepeating("SeekPlayer", 0, 0.1f);

        Destroy(this.gameObject, lifeTime);
    }

    public override void Update()
    {
        if (_playerLockOn == null)
        {
            _rigidbody.AddForce(_originalDirection * speed); //continue to add force in the initial direction
        } else
        {
            _newDirection = _playerLockOn.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_newDirection), Time.deltaTime);

            _rigidbody.AddForce(-_originalDirection * speed); //begin to nullify the original force
            _rigidbody.AddForce(transform.forward * speed); //move forward towards the slight rotation
        }
    }


    //Keeps checking if a player is within the sight radius of the projectile. If it is,
    //lock onto the player and stop checking.
    protected void SeekPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            var player = hitCollider.transform.GetComponent<MasterPlayer>();
            if (player != null)
            {
                _playerLockOn = player;
                speed *= 6;

                CancelInvoke();
            }
        }
    }


    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
