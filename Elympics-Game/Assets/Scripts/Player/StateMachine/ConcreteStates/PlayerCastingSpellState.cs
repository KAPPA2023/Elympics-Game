using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastingSpellState : PlayerState
{
    public PlayerCastingSpellState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("Stan CastingSpell");
        Cursor.lockState = CursorLockMode.Confined;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void PlayerElympicsUpdate()
    {
        base.PlayerElympicsUpdate();

        if (player.isDrawingReleased)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void InputUpdate()
    {
        base.InputUpdate();
    }
}