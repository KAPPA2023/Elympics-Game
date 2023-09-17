using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : ElympicsMonoBehaviour
{
    [Header("Climbing")]
    public float climbSpeed;
    private float desiredClimbSpeed;
    [SerializeField] private bool isHolding;

    //[Header("Input")]
    private float horizontalInput;
    private float verticalInput;
    [Header("ClimbJumping")]

    public float climbJumpUpForce;
    public float climbJumpBackForce;
    private ElympicsFloat desiredClimbJumpUpForce = new ElympicsFloat(0);
    private ElympicsFloat desiredClimbJumpBackForce = new ElympicsFloat(0);

    [Header("Detection")]
    public float wallCheckDistance;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private RaycastHit lastFrontWallHit;
    private bool wallFront;

    private Transform lastWall;
    private Vector3 lastWallNormal;

    [Header("References")]
    public Transform orientation;
    private MovementController movementController;
    private Rigidbody rb;
    public LayerMask whatIsWall;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementController = GetComponent<MovementController>();
        isHolding = false;

        desiredClimbJumpUpForce.Value = climbJumpUpForce;
        desiredClimbJumpBackForce.Value = climbJumpBackForce;
        desiredClimbSpeed = climbSpeed;
    }

    public void ClimbingElympicsUpdate(bool isJump, bool shiftPressed, bool shiftReleased)
    {
        horizontalInput = movementController.horizontalInput; 
        verticalInput = movementController.verticalInput;

        CheckForWall();
        StateMachine(isJump, shiftPressed, shiftReleased);
    }

    private void CheckForWall()
    {
        Vector3 rayPos = transform.position;
        rayPos.y -= 0.9f;

        wallFront = Physics.Raycast(transform.position, orientation.forward, out frontWallHit, wallCheckDistance, whatIsWall);
        if (wallFront)
        {
            lastFrontWallHit = frontWallHit;
        }
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
    }

    /*private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }*/

    private void StateMachine(bool isJump, bool shiftPressed, bool shiftReleased)
    {
        if (movementController.isClimbing && shiftPressed)
        {
            isHolding = true;
            rb.velocity = new Vector3(0, 0, 0);
        } 

        if (isHolding && shiftReleased) 
        {
            isHolding = false;
        }

        if (wallFront && verticalInput > 0 && wallLookAngle < maxWallLookAngle)
        {
            if (!movementController.isClimbing)
            {
                StartClimbing();
            } 
        }
        else if (!isHolding)
        {
            if (movementController.isClimbing)
            {
                StopClimbing();
            }
        }

        if ((wallFront && isJump) || (isJump && isHolding))
        {
            ClimbJump();
        }
    }

    private void StartClimbing()
    {
        movementController.isClimbing = true;
        rb.useGravity = false;
        rb.velocity = new Vector3(0f, 0f, 0f);
    }

    public void ClimbingMovement()
    {
        if (!isHolding)
        {
            rb.velocity = new Vector3(rb.velocity.x, desiredClimbSpeed, rb.velocity.z);
        }
        
    }

    private void ClimbJump()
    {
        Vector3 forceToApply = transform.up * desiredClimbJumpUpForce.Value + lastFrontWallHit.normal * desiredClimbJumpBackForce.Value;
        isHolding = false;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    public void SetClimbJumpForces(float climbUpForce, float climbBackForce)
    {
        desiredClimbJumpUpForce.Value = climbUpForce;
        desiredClimbJumpBackForce.Value = climbBackForce;
    }

    public void SetClimbSpeed(float speed)
    {
        desiredClimbSpeed = speed;
    }

    public void ResetClimbJumpForces()
    {
        desiredClimbJumpUpForce.Value = climbJumpUpForce;
        desiredClimbJumpBackForce.Value = climbJumpBackForce;
    }

    private void StopClimbing()
    {
        movementController.isClimbing = false;
        rb.useGravity = true;
    }

    public bool getIsHolding()
    {
        return isHolding;
    }


}
