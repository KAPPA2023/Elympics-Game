using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using WobbrockLib;
using WobbrockLib.Extensions;
using Debug = UnityEngine.Debug;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private ShapeInput _shapeInput;
    private List<TimePointF> _points = new List<TimePointF>(255);
    private GatheredInput _gatheredInput;

    public void Start()
    {
        _gatheredInput.shape = -1;
        _gatheredInput.attack_triggered = false;
    }

    public void UpdateInput()
    {
        Vector2 moveDirections = _playerInput.actions["Move"].ReadValue<Vector2>();
        _gatheredInput.movementInput = moveDirections;
        HandleSpellDrawing();
        if (_playerInput.actions["CastSpell"].WasPressedThisFrame())
        {
            _gatheredInput.attack_triggered = true;
        }
    }

    private void HandleSpellDrawing()
    {
        Vector2 position;
        int returned_shape = -1;
        if (_playerInput.actions["DrawShape"].IsPressed())
        {
            position = Input.mousePosition;
            _points.Add(new TimePointF(position.x, position.y, TimeEx.NowMs));
        }
        else if (_playerInput.actions["DrawShape"].WasReleasedThisFrame())
        {
            returned_shape = _shapeInput.GetShape(_points);
            _gatheredInput.shape = returned_shape;
            _points.Clear();
        }
    }

    public GatheredInput getInput()
    {
        GatheredInput returnedInput = _gatheredInput;
        _gatheredInput.movementInput = Vector2.zero;
        _gatheredInput.shape = -1;
        _gatheredInput.attack_triggered = false;
        return returnedInput;
    }
}

public struct GatheredInput
{
    public Vector2 movementInput;
    public int shape;
    public bool attack_triggered;
    //public Vector3 mousePosition;
}
