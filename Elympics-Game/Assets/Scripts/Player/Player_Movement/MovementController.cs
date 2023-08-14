using Elympics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementController : ElympicsMonoBehaviour, IUpdatable
{
    private new Rigidbody rb = null;

    public float horizontalInput;
    public float verticalInput;
    private ElympicsFloat SlowTimer =new ElympicsFloat(0.0f);
    private ElympicsFloat SlowTime = new ElympicsFloat(10);
    private bool isSlow=false;
    #region Speeds and Drags
    [SerializeField] private float GroundSpeed;
    [SerializeField] private float WallRunSpeed;
    public ElympicsFloat desiredMovementSpeed;
    [SerializeField] public float actualMovementSpeed;
    [SerializeField] private float groundDrag;
    #endregion
    
    #region Jumping Variables
    public float jumpForce;
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
    private Climbing wallRunning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wallRunning = GetComponent<Climbing>();
        desiredMovementSpeed.Value = GroundSpeed;
    }

    public void ProcessMovement(Vector2 inputMovement, bool jump)
    {
        horizontalInput = inputMovement.x;
        verticalInput = inputMovement.y;
        

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
            
            wallRunning.WallRunningMovement();
        }
        else if (GroundCheck())
        {
            rb.drag = groundDrag;
            
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

        /*if (isWallRunning)
        {
            if (rb.velocity.y > desiredMovementSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, desiredMovementSpeed, rb.velocity.z);
            }
            actualMovementSpeed = rb.velocity.y;
        }
        else */if (flatVel.magnitude > desiredMovementSpeed)
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
    
    public void ElympicsStart()
    {
        
        desiredMovementSpeed.Value = GroundSpeed;

    }

   

    public void Slow(float newvalue)
    {
        desiredMovementSpeed.Value -= newvalue;
        if (desiredMovementSpeed.Value != GroundSpeed)
        {
            isSlow = true;
            SlowTime.Value = 0.0f;
        }
       
    }
    
    public void ElympicsUpdate()
    {
        if(isSlow)
        {
            SlowTime.Value += Elympics.TickDuration;
            if (SlowTime.Value >= 4f)
            {
                isSlow = false;
                desiredMovementSpeed.Value = GroundSpeed;
                SlowTime.Value = 0f;
            }
        }
    }
}
