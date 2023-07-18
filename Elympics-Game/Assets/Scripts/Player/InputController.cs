using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class InputController : ElympicsMonoBehaviour, IInputHandler, IUpdatable
{
    [SerializeField] private PlayerData _playerData = null;
    [SerializeField] private InputProvider _inputHandler;
    [SerializeField] private MovementController _movementHandler;
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
            inputReader.Read(out x1);
            inputReader.Read(out y1);
            inputReader.Read(out spaceClicked);
            inputReader.Read(out attack_triggered);
            inputReader.Read(out shape);
            currentInput.movementInput = new Vector2(x1, y1);
            currentInput.jumpInput = spaceClicked;
            currentInput.shape = shape;
            currentInput.attack_triggered = attack_triggered;
        }
        _movementHandler.ProcessMovement(currentInput.movementInput, currentInput.jumpInput);
        if(currentInput.attack_triggered) _spellManager.castSpell();
        _spellManager.chooseSpell(currentInput.shape);
        
    }
}
