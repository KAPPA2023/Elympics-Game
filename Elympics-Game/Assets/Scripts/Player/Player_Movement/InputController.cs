using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using Unity.Profiling.Editor;

public class InputController : ElympicsMonoBehaviour, IInputHandler, IUpdatable
{
    private PlayerController player = null;
    [SerializeField] private PlayerData _playerData = null;
    [SerializeField] private InputProvider _inputHandler;
    [SerializeField] private MovementController _movementHandler;
    [SerializeField] private ActionHandler _actionHandler;
    [SerializeField] private ViewController _viewController;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
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
        currentInput.jumpInput = false;
        currentInput.attack_triggered = false;
        currentInput.shape = -1;

        if (ElympicsBehaviour.TryGetInput(ElympicsPlayer.FromIndex(_playerData.PlayerId), out var inputReader))
        {
            float x1, y1;
            bool spaceClicked;
            bool attack_triggered;
            int shape;
            float xRotation, yRotation, zRotation;
            inputReader.Read(out x1);
            inputReader.Read(out y1);
            inputReader.Read(out xRotation);
            inputReader.Read(out yRotation);
            inputReader.Read(out zRotation);
            inputReader.Read(out spaceClicked);
            inputReader.Read(out attack_triggered);
            inputReader.Read(out shape);
            currentInput.movementInput = new Vector2(x1, y1);
            currentInput.jumpInput = spaceClicked;
            currentInput.shape = shape;
            currentInput.attack_triggered = attack_triggered;

            player.mouseRotation = Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation));
            player.ElympicsUpdate();
            //ProcessMouse(Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation)));
        }
        _movementHandler.ProcessMovement(currentInput.movementInput, currentInput.jumpInput);
        _actionHandler.HandleActions(currentInput.attack_triggered,false, currentInput.shape, Elympics.Tick);
    }

    private void ProcessMouse(Quaternion mouseRotation)
    {
        _viewController.ProcessView(mouseRotation);
    }
    
}
