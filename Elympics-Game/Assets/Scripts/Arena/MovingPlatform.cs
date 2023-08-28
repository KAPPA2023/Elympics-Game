using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MovingPlatform : ElympicsMonoBehaviour, IUpdatable, IInitializable
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 0.1f;
    private Vector3 direction;

    private int _currentPoint = 0;
    //[SerializeField] private Rigidbody rb;
    private Vector3 shift;
    private List<MovementController> movementControllers;
    public void Initialize()
    {
        direction = points[1].position - points[0].position;
        direction.Normalize();
        shift = Vector3.zero;
        movementControllers = new List<MovementController>();
    }

    public void ElympicsUpdate()
    {
        var currentPos = transform.position;
        var nextPatrolPoint = points[_currentPoint].position;
        if (Vector3.Distance(currentPos, nextPatrolPoint) > 0.1f)
        {
            MoveTowardsPoint(points[_currentPoint].position);
        }
        else
        {
            _currentPoint = (_currentPoint + 1) % points.Length;
            direction *= -1;
            shift = Vector3.zero;
        }
        HandlePlayers();
    }

    void MoveTowardsPoint(Vector3 point)
    {
        // Move towards target
        //rb.velocity = direction * speed;
        shift = Vector3.MoveTowards(transform.position, point, speed) - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, point, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            movementControllers.Add(other.GetComponent<MovementController>()); 
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            movementControllers.Remove(other.GetComponent<MovementController>());
        }
    }
    
    void HandlePlayers()
    {
        foreach (var player in movementControllers)
        {
            player.Move(shift);
        }
    }

}
