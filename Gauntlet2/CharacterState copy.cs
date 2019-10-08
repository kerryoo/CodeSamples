using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState
{
    protected Character character;
    protected InputControl inputControl;
    protected GameObject gameObject;
    public int stateID { get; protected set; }

    //each character state will handle input differently, which can be
    //duck typed with a CharacterState call in the Character class
    public abstract int handleInput();

    protected CharacterState(Character character, InputControl inputControl, GameObject gameObject)
    {
        this.character = character;
        this.inputControl = inputControl;
        this.gameObject = gameObject;
    }
  
}
