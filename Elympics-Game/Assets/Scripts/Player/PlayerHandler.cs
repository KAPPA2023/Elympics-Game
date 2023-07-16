using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerHandler : ElympicsMonoBehaviour, IInputHandler, IUpdatable
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private MovementHandler _movementHandler;
    [SerializeField] private SpellManager _spellManager;

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
        currentInput.attack_triggered = false;
        currentInput.shape = -1;

        if (ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader))
        {
            float x1, y1;
            bool attack_triggered;
            int shape;
            inputReader.Read(out x1);
            inputReader.Read(out y1);
            inputReader.Read(out attack_triggered);
            inputReader.Read(out shape);
            currentInput.movementInput = new Vector2(x1, y1);
            currentInput.shape = shape;
            currentInput.attack_triggered = attack_triggered;
        }
        _movementHandler.HandleMovement(currentInput.movementInput);
        if(currentInput.attack_triggered) _spellManager.castSpell();
        _spellManager.chooseSpell(currentInput.shape);
        
    }
}
