using Elympics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementController : ElympicsMonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] public float actualMovementSpeed;

    [SerializeField] private float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true;
    private new Rigidbody rb = null;

    #region Is Player On Ground Variables
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ProcessMovement(Vector2 inputMovement, bool jump)
    {
        Vector3 inputVector = new Vector3(inputMovement.x, 0, inputMovement.y);
        Vector3 movementDirection = inputVector != Vector3.zero ? this.transform.TransformDirection(inputVector.normalized) : Vector3.zero;

        ApplyMovement(movementDirection);
        SpeedControl();

        if (GroundCheck())
        {
            rb.drag = groundDrag;
            if (jump && readyToJump)
            {
                readyToJump = false;

                ApplyJump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        } else {
            rb.drag = 1;
        }


        
    }

    private void ApplyMovement(Vector3 movementDirection)
    {
        Vector3 defaultVelocity = movementDirection * movementSpeed * 10f;
        if (!GroundCheck())
        {
            defaultVelocity *= airMultiplier;
        }

        rb.AddForce(defaultVelocity, ForceMode.Force);
    }

    private void ApplyJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        actualMovementSpeed = flatVel.magnitude;
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
