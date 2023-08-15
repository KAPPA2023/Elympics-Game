using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class MovingPlatform : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 0.1f;
    private Vector3 direction;
    private Vector3 shift;

    private int _currentPoint = 0;
    //[SerializeField] private Rigidbody rb;
    private MovementController movementController;

    private void Start()
    { 
        direction = points[1].position - points[0].position;
        direction.Normalize();
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
        }
    }

    void MoveTowardsPoint(Vector3 point)
    {
        // Move towards target
        //rb.velocity = direction * speed;
        shift += Vector3.MoveTowards(transform.position, point, speed) - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, point, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            movementController = other.GetComponent<MovementController>();
            shift = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            movementController.Move(shift);
            shift = Vector3.zero;
        }
    }

}
