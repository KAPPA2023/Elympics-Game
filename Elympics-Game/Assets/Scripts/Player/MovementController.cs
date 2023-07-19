using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : ElympicsMonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;

    private new Rigidbody _playerRigidbody = null;

    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    public void ProcessMovement(Vector2 inputMovement, bool jump)
    {
        Vector3 inputVector = new Vector3(inputMovement.x, 0, inputMovement.y);
        Vector3 movementDirection = inputVector != Vector3.zero ? this.transform.TransformDirection(inputVector.normalized) : Vector3.zero;

        ApplyMovement(movementDirection);

        if (jump && GroundCheck())
        {
            ApplyJump();
        }
    }

    private void ApplyMovement(Vector3 movementDirection)
    {
        Vector3 defaultVelocity = movementDirection * _movementSpeed * Elympics.TickDuration;

        _playerRigidbody.velocity = new Vector3(defaultVelocity.x, _playerRigidbody.velocity.y, defaultVelocity.z);
    }

    private void ApplyJump()
    {
        _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private bool GroundCheck()
    {
        if (Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance, layerMask))
        {
            return true;
        } else 
        { 
            return false; 
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }

}
