using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;

public class InputProvider : MonoBehaviour
{
    [SerializeField] private ShapeInput _shapeInput;
    private List<Point> _points = new List<Point>();
    private GatheredInput _gatheredInput;
    Vector2 _position;
    #region Mouse Variables
    [SerializeField] private float mouseSensivity;
    [SerializeField] private Vector2 verticalAngleLimits;
    #endregion
    
    public void Start()
    {
        _gatheredInput.shape = (int)Spells.Empty;
        _gatheredInput.attack_triggered = false;
        _gatheredInput.isDrawing = false;
        _position = new Vector2(1, 1);
    }

    public void UpdateInput()
    {
        _gatheredInput.movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _gatheredInput.attack_triggered = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gatheredInput.jumpInput = true;
        }
        HandleSpellDrawing();
        
        if (!Input.GetKey(KeyCode.Mouse1))
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y") * -1;
            var newMouseAngles = _gatheredInput.mouseAxis + new Vector3(mouseY, mouseX) * mouseSensivity;
            _gatheredInput.mouseAxis = FixTooLargeMouseAngles(newMouseAngles);
        }
        
    }
   
    private Vector3 FixTooLargeMouseAngles(Vector3 mouseAngles)
    {
        mouseAngles.x = Mathf.Clamp(mouseAngles.x, verticalAngleLimits.x, verticalAngleLimits.y);

        return mouseAngles;
    }

    private void HandleSpellDrawing()
    {
        string returned_shape = "empty";
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _gatheredInput.isDrawing = true;
        }
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            int x = (Screen.width * 2/5);
            int y = Screen.height * 3/10 ;

            if (_position.x != Input.mousePosition.x || _position.y != Input.mousePosition.y)
            {
                _position = Input.mousePosition;
                _points.Add(new Point(_position.x - x, (Screen.height - _position.y) - y,0));
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _gatheredInput.isDrawingReleased = true;
            if(_points.Count > 1)
                _gatheredInput.shape = (int)_shapeInput.GetShape(_points);
            _points.Clear();
        }

    }

    public GatheredInput getInput()
    {
        GatheredInput returnedInput = _gatheredInput;
        _gatheredInput.movementInput = Vector2.zero;
        _gatheredInput.jumpInput = false;
        _gatheredInput.shape = (int)Spells.Empty;
        _gatheredInput.attack_triggered = false;
        _gatheredInput.isDrawing = false;
        _gatheredInput.isDrawingReleased = false;
        return returnedInput;
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
