using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawingSpellState : PlayerState
{
    public PlayerDrawingSpellState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
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

        if (player.isDead())
        {
            stateMachine.ChangeState(player.DeadState);
        }

        if (player.isDrawingReleased)
        {
            stateMachine.ChangeState(player.NormalState);
        }

        player.MoveAround();
        player.Shoot();
        player.CheckDrawedSpell();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();
    }
}
