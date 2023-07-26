using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEngine.SceneManagement;

public class InputController : ElympicsMonoBehaviour, IInputHandler, IUpdatable
{
    private PlayerController player = null;
    [SerializeField] private PlayerData _playerData = null;
    [SerializeField] private InputProvider _inputHandler;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ActionHandler _actionHandler;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        gameManager.PostGameTime.ValueChanged += BacktoMenu;
    }

    private void Update()
    {
        if(Elympics.Player != PredictableFor) return;
        _inputHandler.UpdateInput();
    }

    public void OnInputForClient(IInputWriter inputSerializer)
    {
        GatheredInput currentInput = _inputHandler.getInput();
        inputSerializer.Write(currentInput.movementInput.x);
        inputSerializer.Write(currentInput.movementInput.y);
        inputSerializer.Write(currentInput.mouseAxis.x);
        inputSerializer.Write(currentInput.mouseAxis.y);
        inputSerializer.Write(currentInput.mouseAxis.z);
        inputSerializer.Write(currentInput.jumpInput);
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
        if (gameManager.gameEnded.Value) return;
        
        GatheredInput currentInput;
        currentInput.movementInput = Vector2.zero;
        currentInput.jumpInput = false;
        currentInput.attack_triggered = false;
        currentInput.shape = "empty";
        currentInput.isDrawing = false;

        if (ElympicsBehaviour.TryGetInput(ElympicsPlayer.FromIndex(_playerData.PlayerId), out var inputReader))
        {
            float x1, y1;
            bool spaceClicked;
            bool attack_triggered;
            bool isDrawing, isDrawingReleased;
            string shape;
            float xRotation, yRotation, zRotation;
            inputReader.Read(out x1);
            inputReader.Read(out y1);
            inputReader.Read(out xRotation);
            inputReader.Read(out yRotation);
            inputReader.Read(out zRotation);
            inputReader.Read(out spaceClicked);
            inputReader.Read(out attack_triggered);
            inputReader.Read(out isDrawing);
            inputReader.Read(out isDrawingReleased);
            inputReader.Read(out shape);
            currentInput.movementInput = new Vector2(x1, y1);
            currentInput.jumpInput = spaceClicked;
            currentInput.shape = shape;
            currentInput.attack_triggered = attack_triggered;
            currentInput.isDrawing = isDrawing;

            player.mouseRotation = Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation));
            player.isDrawingReleased = isDrawingReleased;
            player.isDrawingPressed = isDrawing;
            player.movementInput = currentInput.movementInput;
            player.isJump = currentInput.jumpInput;
            player.attackTriggered = attack_triggered;
            player.shape = shape;

            player.PlayerElympicsUpdate();
            //_actionHandler.HandleActions(currentInput.attack_triggered, currentInput.shape);
        }
        
    }
    
    //TODO: remove this
    private void BacktoMenu(float oldVal, float newValue)
    {
        if(Elympics.IsServer) return;
        if (newValue < 0.1f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
    
}
