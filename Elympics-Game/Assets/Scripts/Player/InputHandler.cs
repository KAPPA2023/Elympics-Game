using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    private GatheredInput _gatheredInput;
    public void UpdateInput()
    {
        Vector2 moveDirections = _playerInput.actions["Move"].ReadValue<Vector2>();
        _gatheredInput.movementInput = moveDirections;
    }

    public GatheredInput getInput()
    {
        GatheredInput returnedInput = _gatheredInput;
        _gatheredInput.movementInput = Vector2.zero;
        return returnedInput;
    }
}

public struct GatheredInput
{
    public Vector2 movementInput;
    //public Vector3 mousePosition;
}
