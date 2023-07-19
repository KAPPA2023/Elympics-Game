using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerInfo : MonoBehaviour, IObservable, IInitializable
{
    [Header("Parameters:")]
    [SerializeField] private float maxHealth;
    [SerializeField] private ElympicsFloat health = new ElympicsFloat();
    [SerializeField] private ActionHandler ActionHandler;
    
    public void DealDamage(float damage)
    {
        health.Value -= damage;
        Debug.Log(health.Value);
    }

    public void Initialize()
    {
        health.Value = maxHealth;
    }
}
