using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateLibrary : MonoBehaviour
{
    public CharacterStateLibrary(InputControl inputControl)
    {
        this._inputControl = inputControl;
    }

    private CharacterState _currCharacterState;
    private int _currStateID;
    private InputControl _inputControl;
    
    private IdleState m_IdleState;
    public IdleState IdleState
    {
        get
        {
            if (m_IdleState == null)
            {
                m_IdleState = new IdleState(_inputControl, gameObject);
            }
            return m_IdleState;
        }
    }

    private WalkingState m_WalkingState;
    public WalkingState WalkingState
    {
        get
        {
            if (m_WalkingState == null)
            {
                m_WalkingState = new WalkingState(_inputControl, gameObject);
            }
            return m_WalkingState;
        }
    }

    private SpecialMovementState m_SpecialMovementState;
    public SpecialMovementState SpecialMovementState
    {
        get
        {
            if (m_SpecialMovementState == null)
            {
                m_SpecialMovementState = new SpecialMovementState(_inputControl, gameObject);
            }
            return m_SpecialMovementState;
        }
    }

    private ShieldingState m_ShieldingState;
    public ShieldingState ShieldingState
    {
        get
        {
            if (m_ShieldingState == null)
            {
                m_ShieldingState = new ShieldingState(_inputControl, gameObject);
            }
            return m_ShieldingState;
        }
    }

    private JumpingState m_JumpingState;
    public JumpingState JumpingState
    {
        get
        {
            if (m_JumpingState == null)
            {
                m_JumpingState = new JumpingState(_inputControl, gameObject);
            }
            return m_JumpingState;
        }
    }

    public void handleInput()
    {
        _currStateID = _currCharacterState.handleInput();
        switchCharacterState();

    }

    private void switchCharacterState()
    {
        switch (_currStateID)
        {
            case SwitchID.Idle:
                _currCharacterState = IdleState;
                break;
            case SwitchID.Walking:
                _currCharacterState = WalkingState;
                break;
            case SwitchID.SpecialMovement:
                _currCharacterState = SpecialMovementState;
                break;
            case SwitchID.Shielding:
                _currCharacterState = ShieldingState;
                break;
            case SwitchID.Jumping:
                _currCharacterState = JumpingState;
                break;
        }
    }

}
