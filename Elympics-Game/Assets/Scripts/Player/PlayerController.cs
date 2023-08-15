using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region State Machine Variables
    private ViewController viewController;
    private MovementController movementController;
    private DeathController deathController;
    private ActionHandler actionHandler;
    private PlayerData playerData;
    public PlayerStateMachine StateMachine { get; set; }
    public PlayerNormalState NormalState { get; set; }
    public PlayerDrawingSpellState DrawingSpellState { get; set; }
    public PlayerDeadState DeadState { get; set; }


    #endregion

    #region DrawingSpell Variables

    public bool isDrawingReleased = false;
    public Spells shape;

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
        deathController = GetComponent<DeathController>();
        StateMachine = new PlayerStateMachine();
        NormalState = new PlayerNormalState(this, StateMachine, deathController);
        DrawingSpellState = new PlayerDrawingSpellState(this, StateMachine, deathController);
        DeadState = new PlayerDeadState(this, StateMachine, deathController);
        actionHandler = GetComponent<ActionHandler>();
    }


    void Start()
    {
        StateMachine.Initialize(NormalState);
        viewController = GetComponent<ViewController>();
        movementController = GetComponent<MovementController>();
        playerData = GetComponent<PlayerData>();
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
        actionHandler.ChooseSpell(shape);
    }

    #endregion
}