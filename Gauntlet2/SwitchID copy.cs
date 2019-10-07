using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SwitchID
{
    public const int StateBuffer = 1000;
    public const int CrowdControlBuffer = 2000;
    public const int CharacterBuffer = 3000;

    public enum PossibleStates
    {
        Idle,
        Walking,
        WalkingBack,
        SpecialMovement,
        Shielding,
        Jumping
    }


    public const int Idle = StateBuffer + (int)PossibleStates.Idle;
    public const int Walking = StateBuffer + (int)PossibleStates.Walking;
    public const int WalkingBack = StateBuffer + (int)PossibleStates.WalkingBack;
    public const int SpecialMovement = StateBuffer + (int)PossibleStates.SpecialMovement;
    public const int Shielding = StateBuffer + (int)PossibleStates.Shielding;
    public const int Jumping = StateBuffer + (int)PossibleStates.Jumping;

    public enum PossibleCrowdControl
    {
        Stun,
        Slow,
        Silence,
        Disarm,
        Snare
    }

    public const int Stun = CrowdControlBuffer + (int)PossibleCrowdControl.Stun;
    public const int Slow = CrowdControlBuffer + (int)PossibleCrowdControl.Slow;
    public const int Silence = CrowdControlBuffer + (int)PossibleCrowdControl.Silence;
    public const int Disarm = CrowdControlBuffer + (int)PossibleCrowdControl.Disarm;

    public enum CharacterID
    {
        Isaac,
        Firefly,
        Kassandra,
        Lutece,
        Dexter,
        Cairne,
        XJTen,
        Terra
    }

    public const int Isaac = CharacterBuffer + (int)CharacterID.Isaac;
    public const int Firefly = CharacterBuffer + (int)CharacterID.Firefly;
    public const int Kassandra = CharacterBuffer + (int)CharacterID.Kassandra;
    public const int Lutece = CharacterBuffer + (int)CharacterID.Lutece;
    public const int Dexter = CharacterBuffer + (int)CharacterID.Dexter;
    public const int Cairne = CharacterBuffer + (int)CharacterID.Cairne;
    public const int XJTen = CharacterBuffer + (int)CharacterID.XJTen;
    public const int Terra = CharacterBuffer + (int)CharacterID.Terra;
}
