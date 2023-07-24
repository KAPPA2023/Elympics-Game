using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using WobbrockLib;
using WobbrockLib.Extensions;

public class InputProvider : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private ShapeInput _shapeInput;
    private List<TimePointF> _points = new List<TimePointF>(255);
    private GatheredInput _gatheredInput;
    #region Mouse Variables
    [SerializeField] private float mouseSensivity = 1.5f;
    [SerializeField] private Vector2 verticalAngleLimits = Vector2.zero;
    #endregion

    public void Start()
    {
        _gatheredInput.shape = -1;
        _gatheredInput.attack_triggered = false;
        _gatheredInput.isDrawing = false;
    }

    public void UpdateInput()
    {
        Vector2 moveDirections = _playerInput.actions["Move"].ReadValue<Vector2>();
        _gatheredInput.movementInput = moveDirections;
        HandleSpellDrawing();

        if (checkIfPressedThisFrame("CastSpell"))
        {
            _gatheredInput.attack_triggered = true;
        }

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y") * -1;
        var newMouseAngles = _gatheredInput.mouseAxis + new Vector3(mouseY, mouseX) * mouseSensivity;
        _gatheredInput.mouseAxis = FixTooLargeMouseAngles(newMouseAngles);

        if (checkIfPressedThisFrame("Space"))
        {
            _gatheredInput.jumpInput = true;
        }
    }

    private Vector3 FixTooLargeMouseAngles(Vector3 mouseAngles)
    {
        mouseAngles.x = Mathf.Clamp(mouseAngles.x, verticalAngleLimits.x, verticalAngleLimits.y);

        return mouseAngles;
    }

    private void HandleSpellDrawing()
    {
        Vector2 position;
        int returned_shape = -1;
        if (_playerInput.actions["DrawShape"].WasPressedThisFrame())
        {
            _gatheredInput.isDrawing = true;
        }
        if (_playerInput.actions["DrawShape"].IsPressed())
        {
            position = Input.mousePosition;
            _points.Add(new TimePointF(position.x, position.y, TimeEx.NowMs));
        }
        else if (_playerInput.actions["DrawShape"].WasReleasedThisFrame())
        {
            _gatheredInput.isDrawingReleased = true;
            returned_shape = _shapeInput.GetShape(_points);
            _gatheredInput.shape = returned_shape;
            _points.Clear();
        }
    }

    public GatheredInput getInput()
    {
        GatheredInput returnedInput = _gatheredInput;
        _gatheredInput.movementInput = Vector2.zero;
        _gatheredInput.jumpInput = false;
        _gatheredInput.shape = -1;
        _gatheredInput.attack_triggered = false;
        _gatheredInput.isDrawing = false;
        _gatheredInput.isDrawingReleased = false;
        return returnedInput;
    }

    private bool checkIfPressedThisFrame(string key)
    {
        return _playerInput.actions[key].WasPressedThisFrame();
    }
}

public struct GatheredInput
{
    public Vector2 movementInput;
    public bool jumpInput;
    public bool isDrawing;
    public bool isDrawingReleased;
    public int shape;
    public bool attack_triggered;
    public Vector3 mouseAxis;
}
