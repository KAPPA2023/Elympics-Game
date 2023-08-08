using Elympics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementController : ElympicsMonoBehaviour
{
    private new Rigidbody rb = null;

    public float horizontalInput;
    public float verticalInput;


    #region Speeds and Drags
    [SerializeField] private float GroundSpeed;
    [SerializeField] private float WallRunSpeed;
    private float desiredMovementSpeed;
    [SerializeField] public float actualMovementSpeed;
    [SerializeField] private float groundDrag;
    #endregion
    
    #region Jumping Variables
    [SerializeField] private float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true;
    #endregion

    #region Is Player On Ground Variables
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    #endregion

    public bool isWallRunning = false;
    private WallRunning wallRunning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wallRunning = GetComponent<WallRunning>();
        desiredMovementSpeed = GroundSpeed;
    }

    public void ProcessMovement(Vector2 inputMovement, bool jump)
    {
        horizontalInput = inputMovement.x;
        verticalInput = inputMovement.y;
        desiredMovementSpeed = GroundSpeed;

        Vector3 inputVector = new Vector3(inputMovement.x, 0, inputMovement.y);
        Vector3 movementDirection = inputVector != Vector3.zero ? this.transform.TransformDirection(inputVector.normalized) : Vector3.zero;

        wallRunning.WallRunningElympicsUpdate();
        if (!isWallRunning)
        {
            ApplyMovement(movementDirection);
        }
        
        SpeedControl();      

        if (isWallRunning)
        {
            desiredMovementSpeed = WallRunSpeed;
            wallRunning.WallRunningMovement();
        }
        else if (GroundCheck())
        {
            rb.drag = groundDrag;
            desiredMovementSpeed = GroundSpeed;
            if (jump && readyToJump)
            {
                readyToJump = false;

                ApplyJump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        } else
        {
            rb.drag = 0;
        }
        
    }

    private void ApplyMovement(Vector3 movementDirection)
    {
        Vector3 defaultVelocity = movementDirection * desiredMovementSpeed * 15f * rb.mass;
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

        if (flatVel.magnitude > desiredMovementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * desiredMovementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
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
