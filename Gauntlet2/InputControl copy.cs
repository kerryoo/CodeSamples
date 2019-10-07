using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public Vector2 Direction;
    public Vector2 MouseInput { get; private set; }

    public bool Fire1 { get; private set; }
    public bool AimingOn { get; private set; }
    public bool Testbutton { get; private set; }

    public bool Jumping { get; private set; }
    public bool Shield { get; private set; }
    public bool SpecialAbility { get; private set; }
    public bool CharacterInvOpen { get; private set; }
    public bool Firing { get; private set; }

    private bool _lockUpdate;
    private bool _lockAttack;
    private bool _lockSpecial;

    void Start()
    {
        Direction = new Vector2();
        MouseInput = new Vector2();
    }

    void Update()
    {
        cameraUpdates();
        if (!_lockUpdate)
        {
            gameplayUpdates();
        }
    }

    private void cameraUpdates()
    {
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        AimingOn = Input.GetButton("Fire2");
    }

    private void gameplayUpdates()
    {
        if (!_lockAttack)
        {
            Fire1 = Input.GetButton("Fire1");
        }

        if (!_lockSpecial)
        {
            SpecialAbility = Input.GetKeyDown(KeyCode.R);
        }

        Direction.x = Input.GetAxis("Horizontal");
        Direction.y = Input.GetAxis("Vertical");
    }

    public void lockGameplayInput()
    {
        Direction.x = 0;
        Direction.y = 0;
        Fire1 = false;
        SpecialAbility = false;
        _lockUpdate = true;
    }

    public void lockAttack()
    {
        Fire1 = false;
        _lockAttack = true;
    }

    public void lockSpecial()
    {
        SpecialAbility = false;
        _lockSpecial = true;
    }


    public void unlockGameplayInput()
    {
        _lockUpdate = false;
    }

    public void unlockAttack()
    {
        _lockAttack = false;
    }

    public void unlockSpecial()
    {
        _lockSpecial = false;
    }
}
