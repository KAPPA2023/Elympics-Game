using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : PlayerState
{
    public PlayerNormalState(PlayerController player, PlayerStateMachine stateMachine, DeathController deathController) : base(player, stateMachine, deathController)
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
       
        if (player.isDrawingPressed)
        {
            stateMachine.ChangeState(player.DrawingSpellState);
        }
        player.LookAround();
        player.MoveAround();
        player.Shoot();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();
    }
}
