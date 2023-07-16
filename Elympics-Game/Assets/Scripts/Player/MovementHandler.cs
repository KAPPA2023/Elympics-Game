using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _movementSpeed;
    // Start is called before the first frame update
    public void HandleMovement(Vector2 movementInput)
    {
        movementInput.Normalize();
        movementInput *= _movementSpeed;
        _playerRigidbody.velocity = new Vector3(movementInput.x, 0, movementInput.y);
    }
}
