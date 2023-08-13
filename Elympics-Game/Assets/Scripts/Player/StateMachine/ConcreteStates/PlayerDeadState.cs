using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerController player, PlayerStateMachine stateMachine, DeathController deathController) : base(player, stateMachine, deathController)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Collider>().enabled = false;
    }

    public override void ExitState()
    {
        base.ExitState();
        player.GetComponent<Collider>().enabled = true;
        player.GetComponent<Rigidbody>().useGravity = true;
    }

    public override void PlayerElympicsUpdate()
    {
        base.PlayerElympicsUpdate();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();
    }
}
