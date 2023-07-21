using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void PlayerElympicsUpdate()
    {
        base.PlayerElympicsUpdate();
        //Debug.Log("Idle state");
        if (player.isDrawingPressed)
        {
            stateMachine.ChangeState(player.CastingSpellState);
        }
        player._viewController.ProcessView(player.mouseRotation);
    }

    public override void InputUpdate()
    {
        base.InputUpdate();
    }
}
