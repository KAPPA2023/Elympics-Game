using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Collider>().enabled = false;
        Debug.Log("You're dead");
    }

    public override void ExitState()
    {
        base.ExitState();
        player.GetComponent<Collider>().enabled = true;
        player.GetComponent<Rigidbody>().useGravity = true;
        Debug.Log("You're alive");
    }

    public override void PlayerElympicsUpdate()
    {
        base.PlayerElympicsUpdate();

        if (!player.isDead())
        {
            stateMachine.ChangeState(player.NormalState);
        }
    }

    public override void InputUpdate()
    {
        base.InputUpdate();
    }
}
