using Elympics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class MovementController : ElympicsMonoBehaviour, IUpdatable
{
    private Rigidbody rb = null;

    public float horizontalInput;
    public float verticalInput;


    #region Speeds and Drags
    [SerializeField] private float GroundSpeed;
    [SerializeField] private float WallRunSpeed;
    public ElympicsFloat desiredMovementSpeed;
    [SerializeField] public float actualMovementSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float slowDuration = 4.0f;
    private float slowTimer;
    public ElympicsFloat slowValue = new ElympicsFloat(0);
    public ElympicsBool isSlowed = new ElympicsBool(false);
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

    public bool isClimbing = false;
    private Climbing climbing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        climbing = GetComponent<Climbing>();
        desiredMovementSpeed.Value = GroundSpeed;
        isSlowed.ValueChanged += OnSlow;
    }

    public void ProcessMovement(Vector2 inputMovement, bool jump)
    {
        horizontalInput = inputMovement.x;
        verticalInput = inputMovement.y;

        Vector3 inputVector = new Vector3(inputMovement.x, 0, inputMovement.y);
        Vector3 movementDirection = inputVector != Vector3.zero ? this.transform.TransformDirection(inputVector.normalized) : Vector3.zero;

        climbing.WallRunningElympicsUpdate();
        if (!isClimbing)
        {
            ApplyMovement(movementDirection);
        }
        
        SpeedControl();      

        if (isClimbing)
        {
            
            climbing.WallRunningMovement();
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
        Vector3 defaultVelocity = movementDirection * desiredMovementSpeed.Value * 15f * rb.mass;
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

        if (flatVel.magnitude > desiredMovementSpeed.Value)
        {
            Vector3 limitedVel = flatVel.normalized * desiredMovementSpeed.Value;
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

    public void Slow()
    {
        isSlowed.Value = true;
    }
    
    public void ElympicsUpdate()
    {
        if(isSlowed.Value)
        {
            slowTimer += Elympics.TickDuration;
            if (slowTimer >= slowDuration)
            {
                isSlowed.Value = false;
            }
        }
    }

    private void OnSlow(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            desiredMovementSpeed.Value -= slowValue;
            slowTimer = 0.0f;
        }
        else
        {
            desiredMovementSpeed.Value = GroundSpeed;
            slowTimer = 0f;
        }
    }

    public void Move(Vector3 direction)
    {
        rb.position += direction;
    }

    public void ResetMovement()
    {
        isSlowed.Value = false;
    }

}
