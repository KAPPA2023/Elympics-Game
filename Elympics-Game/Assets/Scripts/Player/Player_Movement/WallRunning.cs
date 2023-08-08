using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : ElympicsMonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit forwardWallHit;
    private bool wallForward;

    [Header("References")]
    public Transform orientation;
    private MovementController movementController;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementController = GetComponent<MovementController>();
    }

    public void WallRunningElympicsUpdate()
    {
        horizontalInput = movementController.horizontalInput; 
        verticalInput = movementController.verticalInput;

        CheckForWall();
        StateMachine();
    }

    private void CheckForWall()
    {
        wallForward = Physics.Raycast(transform.position, orientation.forward, out forwardWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        if (wallForward && verticalInput > 0 && AboveGround())
        {
            if (!movementController.isWallRunning)
            {
                StartWallRun();
            } 
        }
        else if (!wallForward || !AboveGround() || verticalInput <= 0)
        {
            if (movementController.isWallRunning)
            {
                StopWallRun();
            }
        }


    }

    private void StartWallRun()
    {
        movementController.isWallRunning = true;
        rb.useGravity = false;
        rb.velocity = new Vector3(0f, 0f, 0f);
    }

    public void WallRunningMovement()
    {
        Vector3 wallUp = Vector3.up;

        rb.AddForce(wallUp * wallRunForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        movementController.isWallRunning = false;
        rb.useGravity = true;
    }


}
