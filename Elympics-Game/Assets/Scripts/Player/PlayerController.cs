using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region State Machine Variables
    private ViewController viewController;
    private MovementController movementController;
    private ActionHandler actionHandler;
    public PlayerStateMachine StateMachine { get; set; }
    public PlayerNormalState IdleState { get; set; }
    public PlayerDrawingSpellState CastingSpellState { get; set; }


    #endregion

    #region DrawingSpell Variables

    public bool isDrawingReleased = false;
    public string shape;

    #endregion

    #region Idle Variables

    public Quaternion mouseRotation;
    public bool isDrawingPressed = false;
    public Vector2 movementInput;
    public bool isJump;
    public bool attackTriggered;

    #endregion

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerNormalState(this, StateMachine);
        CastingSpellState = new PlayerDrawingSpellState(this, StateMachine);
        actionHandler = GetComponent<ActionHandler>();
    }


    void Start()
    {
        StateMachine.Initialize(IdleState);
        viewController = GetComponent<ViewController>();
        movementController = GetComponent<MovementController>();
    }

    public void PlayerElympicsUpdate()
    {
        StateMachine.CurrentPlayerState.PlayerElympicsUpdate();
    }

    public void InputUpdate()
    {
        StateMachine.CurrentPlayerState.InputUpdate();
    }

    #region View Controller Functions
    public void LookAround()
    {
        viewController.ProcessView(this.mouseRotation);
    }

    #endregion

    #region Movement Controller Functions

    public void MoveAround()
    {
        movementController.ProcessMovement(movementInput, isJump);
    }

    #endregion

    #region Action Controller Functions

    public void Shoot()
    {
        actionHandler.HandleActions(attackTriggered);
    }

    public void CheckDrawedSpell()
    {
        actionHandler.chooseSpell(shape);
    }

    #endregion
}