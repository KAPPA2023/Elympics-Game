using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;
    protected DeathController deathController;

    public PlayerState(PlayerController Player, PlayerStateMachine stateMachine, DeathController deathController)
    {
        this.player = Player;
        this.stateMachine = stateMachine;
        this.deathController = deathController;
    }

    public virtual void EnterState()
    {
        deathController.IsDead.ValueChanged += HandleDeath;
    }

    public virtual void ExitState()
    {
        deathController.IsDead.ValueChanged -= HandleDeath;
    }
    public virtual void PlayerElympicsUpdate() { }
    public virtual void InputUpdate() { }
    
    private void HandleDeath(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            stateMachine.ChangeState(player.DeadState);
        }
        else
        {
            stateMachine.ChangeState(player.NormalState);
        }
    }
}