using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerHandler : ElympicsMonoBehaviour, IInputHandler, IUpdatable
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private MovementHandler _movementHandler;

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
    }

    public void OnInputForBot(IInputWriter inputSerializer)
    {
        //throw new System.NotImplementedException();
    }

    public void ElympicsUpdate()
    {
        GatheredInput currentInput;
        currentInput.movementInput = Vector2.zero;

        if (ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader))
        {
            float x1, y1;
            inputReader.Read(out x1);
            inputReader.Read(out y1);
            currentInput.movementInput = new Vector2(x1, y1);
        }

        _movementHandler.HandleMovement(currentInput.movementInput);
    }
}
