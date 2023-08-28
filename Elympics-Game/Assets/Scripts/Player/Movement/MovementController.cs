using System;
using Elympics;
using UnityEngine;

public class MovementController : ElympicsMonoBehaviour
{
    private Rigidbody rb = null;
    private float playerHeight = 2.0f;
    private Vector3 movementDirection;
    public bool isGrounded = false;

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;

    #region Speeds and Drags
    [Header("Speeds")]
    [SerializeField] private float GroundSpeed;
    [SerializeField] private float ClimbingSpeed;
    [SerializeField] private float SlowSpeed;
    private ElympicsFloat desiredMovementSpeed = new ElympicsFloat(0);

    [SerializeField] public float actualMovementSpeed;
    [SerializeField] private float groundDrag;
    #endregion

    #region Slow Variables
    public ElympicsFloat slowValue = new ElympicsFloat();
    public ElympicsFloat remainingSlow = new ElympicsFloat();
    #endregion

    #region Jumping Variables
    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true;
    #endregion

    #region Slope Variables
    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    #endregion

    #region Is Player On Ground Variables
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    #endregion

    public bool isClimbing = false;
    private Climbing climbing;
    
    public event Action<Vector3> MovementValuesChanged;
    public ElympicsBool IsJumping = new ElympicsBool(false);

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        climbing = GetComponent<Climbing>();
        desiredMovementSpeed.Value = GroundSpeed;
    }

    // Movement Elympics Update
    public void ProcessMovement(Vector2 inputMovement, bool jump)
    {
        horizontalInput = inputMovement.x;
        verticalInput = inputMovement.y;

        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection = inputVector != Vector3.zero ? this.transform.TransformDirection(inputVector.normalized) : Vector3.zero;

        if (remainingSlow.Value > 0f)
        {
            remainingSlow.Value -= Elympics.TickDuration;
            desiredMovementSpeed.Value *= slowValue.Value;
        }

        climbing.ClimbingElympicsUpdate();
        if (!isClimbing)
        {
            ApplyMovement(movementDirection);
        }
        
        SpeedControl();      

        if (isClimbing)
        {
            desiredMovementSpeed.Value = ClimbingSpeed;
            climbing.WallRunningMovement();
        }
        else if (GroundCheck())
        {
            rb.drag = groundDrag;
            desiredMovementSpeed.Value = GroundSpeed;

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

    #region Moving Functions
    private void ApplyMovement(Vector3 movementDirection)
    {
        Vector3 defaultVelocity = movementDirection * desiredMovementSpeed.Value * 25f;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * desiredMovementSpeed.Value * 25f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (GroundCheck())
        {
            rb.AddForce(defaultVelocity, ForceMode.Force);
        }
        else
        {
            rb.AddForce(defaultVelocity * airMultiplier, ForceMode.Force);
        }

        rb.useGravity= !OnSlope();
        //TODO: check what values should use
        MovementValuesChanged?.Invoke(movementDirection);
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > desiredMovementSpeed.Value)
            {
                rb.velocity = rb.velocity.normalized * desiredMovementSpeed.Value;
            }
            actualMovementSpeed = rb.velocity.magnitude;
        }
        else
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
    }

    private bool GroundCheck()
    {
        if (Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance, layerMask))
        {
            isGrounded = true;
            IsJumping.Value = false;
            return true;
        } else 
        {
            isGrounded = false;
            return false; 
        }
    }
    public void Move(Vector3 direction)
    {
        rb.position += direction;
    }
    #endregion

    #region Jump Functions
    private void ApplyJump()
    {
        IsJumping.Value = true;
        
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        exitingSlope = false;

        readyToJump = true;
    }
    #endregion

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;
    }

    #region Slow Functions

    public void Slow(float value, float duration)
    {
        slowValue.Value = value;
        remainingSlow.Value = duration;
    }
    
    public void ResetMovement()
    {
        slowValue.Value = 0f;
        remainingSlow.Value = 0f;
    }
    #endregion

    #region Getters

    public float getDesiredMovementSpeed()
    {
        return desiredMovementSpeed.Value;
    }

    #endregion

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }

}
