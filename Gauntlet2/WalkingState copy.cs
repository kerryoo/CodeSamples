using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : CharacterState
{
    private readonly float interpolation = 10;

    private CharacterStats characterStats;
    private Animator animator;
    private Transform transform;

    private float movementSpeed;
    private float currentV;
    private float currentH;

    public WalkingState(InputControl inputControl, GameObject gameObject) : base(inputControl, gameObject)
    {
        characterStats = gameObject.GetComponent<CharacterStats>();
        animator = gameObject.GetComponent<Animator>();
        transform = gameObject.transform;
        movementSpeed = characterStats.MovementSpeed;

        stateID = StateID.Walking;
    }


    public override int handleInput()
    {
        Move();

        if (inputControl.Jumping)
        {
            return StateID.Jumping;
        }

        if (Mathf.Abs(currentV) <= 0.0001 && Mathf.Abs(currentH) <= 0.0001)
        {
            return StateID.Idle;
        }

        return StateID.Walking;
    }

    private void Move()
    {
        Vector2 direction = new Vector2(inputControl.Vertical, inputControl.Horizontal);
        currentV = Mathf.Lerp(currentV, direction.x, Time.deltaTime * interpolation);
        currentH = Mathf.Lerp(currentH, direction.y, Time.deltaTime * interpolation);

        transform.position += transform.forward * currentV * movementSpeed * Time.deltaTime +
            transform.right * currentH * movementSpeed * Time.deltaTime;
    }
}
