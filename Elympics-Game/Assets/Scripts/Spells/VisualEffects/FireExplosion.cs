using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class FireExplosion : ElympicsMonoBehaviour, IUpdatable
{
    private SphereCollider sphere;
    private ElympicsFloat deathTimer = new ElympicsFloat(0.0f);
    private int owner;
    

    public void ElympicsUpdate()
    {
        //sphere.GetComponent<SphereCollider>();
        deathTimer.Value += Elympics.TickDuration;
        if (deathTimer.Value >= 1.0f)
        {
            ElympicsDestroy(gameObject);
        }
        //sphere.GetComponent<SphereCollider>().radius += sphere.GetComponent<SphereCollider>().radius*Elympics.TickDuration;
    }

    public void OnTriggerEnter(Collider other)
    {
        var playerInfo = other.GetComponent<PlayerData>();
        if (playerInfo != null)
        {
            playerInfo.GetComponent<StatsController>().InitializeFire();
        }
    }
    public virtual void SpawnSpell(Vector3 position,int client)
    {
        transform.position = position;
        owner = client;
    }
}

