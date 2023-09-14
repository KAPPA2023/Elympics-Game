using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class FireExplosion : ElympicsMonoBehaviour, IUpdatable
{
    private ElympicsFloat deathTimer = new ElympicsFloat(0f);
    private int owner;
    private ElympicsFloat burnDuration = new ElympicsFloat(0f);
    private float durationTime = 1.0f;
    private float damagePerSecond;

    public void ElympicsUpdate()
    {
        deathTimer.Value += Elympics.TickDuration;
        if (deathTimer.Value >= durationTime)
        {
            ElympicsDestroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        var playerInfo = other.GetComponent<PlayerData>();
        if (playerInfo != null)
        {
            playerInfo.GetComponent<StatsController>().InitializeFire(owner, burnDuration, damagePerSecond);
        }
    }

    public virtual void SpawnSpell(Vector3 position, int client)
    {
        transform.position = position;
        owner = client;
    }

    public void setDurationAndDamage(float value, float damagePerSecond)
    {
        burnDuration.Value = value;
        this.damagePerSecond = damagePerSecond;
    }
}

