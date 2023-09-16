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

    public ElympicsInt _currentPoint = new ElympicsInt(0);
    private Vector3 shift;
    private List<MovementController> movementControllers;
    public void Initialize()
    {
        shift = Vector3.zero;
        movementControllers = new List<MovementController>();
    }

    public void ElympicsUpdate()
    {
        if (!gameObject.activeInHierarchy) return;
        var currentPos = transform.position;
        var nextPatrolPoint = points[_currentPoint.Value].position;
        if (Vector3.Distance(currentPos, nextPatrolPoint) > 0.1f)
        {
            MoveTowardsPoint(points[_currentPoint.Value].position);
        }
        else
        {
            _currentPoint.Value = (_currentPoint.Value + 1) % points.Length;
        }
        HandlePlayers();
    }

    void MoveTowardsPoint(Vector3 point)
    {
        shift = Vector3.MoveTowards(transform.position, point, speed) - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, point, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            movementControllers.Add(other.GetComponent<MovementController>()); 
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
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
