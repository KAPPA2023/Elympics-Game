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
        Cursor.lockState = CursorLockMode.Locked;
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void ElympicsUpdate()
    {
        base.ElympicsUpdate();
        player._viewController.ProcessView(player.mouseRotation);
    }

    public override void InputUpdate()
    {
        base.InputUpdate();
    }
}
