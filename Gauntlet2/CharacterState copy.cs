using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState
{
    protected InputControl inputControl;
    protected GameObject gameObject;
    public int stateID { get; protected set; }

    public abstract int handleInput();

    protected CharacterState(InputControl inputControl, GameObject gameObject)
    {
        this.inputControl = inputControl;
        this.gameObject = gameObject;
    }
  
}
