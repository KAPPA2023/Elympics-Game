using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WobbrockLib.Win32;

public class PlayerCastingSpellState : PlayerState
{
    public PlayerCastingSpellState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Stan CastingSpell");
        Cursor.lockState = CursorLockMode.None;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void ElympicsUpdate()
    {
        base.ElympicsUpdate();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();
    }
}
