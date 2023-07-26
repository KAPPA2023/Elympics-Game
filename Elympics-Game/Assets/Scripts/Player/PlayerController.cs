using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    #region State Machine Variables
    [SerializeField] public ViewController viewController;
    [SerializeField] public MovementController movementController;
    [SerializeField] public ActionHandler actionHandler;
    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerCastingSpellState CastingSpellState { get; set; }


    #endregion

    #region CastingSpell Variables

    public bool isDrawingReleased = false;

    #endregion

    #region Idle Variables

    public Quaternion mouseRotation;
    public bool isDrawingPressed = false;
    public Vector2 movementInput;
    public bool isJump;
    public bool attackTriggered;
    public string shape;

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

    public void PlayerElympicsUpdate()
    {
        StateMachine.CurrentPlayerState.PlayerElympicsUpdate();
    }

    public void InputUpdate()
    {
        StateMachine.CurrentPlayerState.InputUpdate();
    }

}