using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : ElympicsMonoBehaviour
{
    [Header("Climbing")]
    public float climbSpeed;


    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    [Header("References")]
    public Transform orientation;
    private MovementController movementController;
    private Rigidbody rb;
    public LayerMask whatIsWall;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementController = GetComponent<MovementController>();
    }

    public void ClimbingElympicsUpdate()
    {
        horizontalInput = movementController.horizontalInput; 
        verticalInput = movementController.verticalInput;

        CheckForWall();
        StateMachine();
    }

    private void CheckForWall()
    {
        Vector3 rayPos = transform.position;
        rayPos.y -= 0.9f;

        wallFront = Physics.Raycast(transform.position, orientation.forward, out frontWallHit, wallCheckDistance, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
    }

    /*private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }*/

    private void StateMachine()
    {
        if (wallFront && verticalInput > 0 && wallLookAngle < maxWallLookAngle)
        {
            if (!movementController.isClimbing)
            {
                StartClimbing();
            } 
        }
        else if (!wallFront || verticalInput <= 0)
        {
            if (movementController.isClimbing)
            {
                StopClimbing();
            }
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
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    private void StopClimbing()
    {
        movementController.isClimbing = false;
        rb.useGravity = true;
    }


}
