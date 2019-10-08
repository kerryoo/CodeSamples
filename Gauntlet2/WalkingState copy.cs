using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : CharacterState
{
    private float currentV;
    private float currentH;

    public WalkingState(Character character, InputControl inputControl, GameObject gameObject) :
        base(character, inputControl, gameObject)
    {
        stateID = SwitchID.Walking;
    }


    public override int handleInput()
    {
        character.Move();

        if (inputControl.Jumping)
        {
            return SwitchID.Jumping;
        }

        if (Mathf.Abs(currentV) <= 0.0001 && Mathf.Abs(currentH) <= 0.0001)
        {
            return SwitchID.Idle;
        }

        return SwitchID.Walking;
    }
}
