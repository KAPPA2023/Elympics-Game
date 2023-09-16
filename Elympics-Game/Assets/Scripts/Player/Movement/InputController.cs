using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;


public class InputController : ElympicsMonoBehaviour, IInputHandler, IUpdatable
{
    private PlayerController player = null;
    [SerializeField] private PlayerData _playerData = null;
    [SerializeField] private PlayerVote playerVote = null;
    [SerializeField] private InputProvider _inputHandler;
    [SerializeField] private GameManager gameManager;
    public bool isTutorialLevel = false;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!isTutorialLevel)
        {
            if(Elympics.Player != PredictableFor) return;
            _inputHandler.UpdateInput();
        }
        else
        {
            _inputHandler.UpdateInput();
        }
        
    }

    public void OnInputForClient(IInputWriter inputSerializer)
    {
        GatheredInput currentInput = _inputHandler.getInput();
        inputSerializer.Write(currentInput.movementInput.x);
        inputSerializer.Write(currentInput.movementInput.y);
        inputSerializer.Write(currentInput.mouseAxis.x);
        inputSerializer.Write(currentInput.mouseAxis.y);
        inputSerializer.Write(currentInput.mouseAxis.z);
        inputSerializer.Write(currentInput.jumpPressed);
        inputSerializer.Write(currentInput.jumpReleased);
        inputSerializer.Write(currentInput.shiftPressed);
        inputSerializer.Write(currentInput.shiftReleased);
        inputSerializer.Write(currentInput.attack_triggered);
        inputSerializer.Write(currentInput.isDrawing);
        inputSerializer.Write(currentInput.isDrawingReleased);
        inputSerializer.Write(currentInput.shape);
    }

    public void OnInputForBot(IInputWriter inputSerializer)
    {
        //throw new System.NotImplementedException();
    }

    public void ElympicsUpdate()
    {

        GatheredInput currentInput;
        currentInput.movementInput = Vector2.zero;
        currentInput.jumpPressed = false;
        currentInput.jumpReleased = false;
        currentInput.shiftPressed = false;
        currentInput.shiftReleased = false;
        currentInput.attack_triggered = false;
        currentInput.shape = -1;
        currentInput.isDrawing = false;

        if (ElympicsBehaviour.TryGetInput(ElympicsPlayer.FromIndex(_playerData.PlayerId), out var inputReader))
        {
            float x1, y1;
            bool spaceClicked, spaceReleased;
            bool shiftPressed, shiftReleased;
            bool attack_triggered;
            bool isDrawing, isDrawingReleased;
            int shape;
            float xRotation, yRotation, zRotation;

            inputReader.Read(out x1);
            inputReader.Read(out y1);
            inputReader.Read(out xRotation);
            inputReader.Read(out yRotation);
            inputReader.Read(out zRotation);
            inputReader.Read(out spaceClicked);
            inputReader.Read(out spaceReleased);
            inputReader.Read(out shiftPressed);
            inputReader.Read(out shiftReleased);
            inputReader.Read(out attack_triggered);
            inputReader.Read(out isDrawing);
            inputReader.Read(out isDrawingReleased);
            inputReader.Read(out shape);

            currentInput.movementInput = new Vector2(x1, y1);
            currentInput.jumpPressed = spaceClicked;
            currentInput.jumpReleased = spaceReleased;
            currentInput.shiftPressed = shiftPressed;
            currentInput.shiftReleased = shiftReleased;
            currentInput.shape = shape;
            currentInput.attack_triggered = attack_triggered;
            currentInput.isDrawing = isDrawing;

            if (!gameManager.matchTime.Value)
            {
                playerVote.HandleInput(spaceClicked, x1);
                return;
            }

            player.mouseRotation = Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation));
            player.isDrawingReleased = isDrawingReleased;
            player.isDrawingPressed = isDrawing;
            player.movementInput = currentInput.movementInput;
            player.isJumpPressed = currentInput.jumpPressed;
            player.isJumpReleased = currentInput.jumpReleased;
            player.isShiftPressed = currentInput.shiftPressed;
            player.isShiftReleased = currentInput.shiftReleased;
            player.attackTriggered = attack_triggered;
            player.shape = (Spells)shape;

            player.PlayerElympicsUpdate();
        }
        
    }
}
