using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    #region State Machine Variables
    [SerializeField] public ViewController _viewController;
    [SerializeField] public InputProvider _inputProvider;
    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerCastingSpellState CastingSpellState { get; set; }


    #endregion

    #region Idle Variables

    public Quaternion mouseRotation;
    //public float RandomMovementSpeed = 14f;
    //public int direction = 1;

    #endregion

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        CastingSpellState = new PlayerCastingSpellState(this, StateMachine);
    }


    void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    public void ElympicsUpdate()
    {
        StateMachine.CurrentPlayerState.ElympicsUpdate();
    }

    public void InputUpdate()
    {
        StateMachine.CurrentPlayerState.InputUpdate();
    }

}