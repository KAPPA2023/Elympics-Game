using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerController Player, PlayerStateMachine stateMachine)
    {
        this.player = Player;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void PlayerElympicsUpdate() { }
    public virtual void InputUpdate() { }

}