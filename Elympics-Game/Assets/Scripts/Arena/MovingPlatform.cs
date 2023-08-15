using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class MovingPlatform : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 0.1f;

    private int _currentPoint;
    
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
        }
    }
    void MoveTowardsPoint(Vector3 point)
    {
        // Move towards target
        transform.position = Vector3.MoveTowards(transform.position, point, speed);
    }
}
