using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using Unity.VisualScripting;
using UnityEngine;

public class SandStorm : ElympicsMonoBehaviour, IUpdatable
{
    private int owner;
    [SerializeField] private float damage;
    [SerializeField] protected ElympicsFloat lifeTime = new ElympicsFloat(4);
    private List<PlayerData> players = new List<PlayerData>();
    private ElympicsFloat deathTimer = new ElympicsFloat(0.0f);
    private ElympicsFloat damageTimer = new ElympicsFloat(0.0f);
    // Start is called before the first frame update
  
    public virtual void SpawnSpell(Vector3 position,int client)
    {
        transform.position = position;
        owner = client;
    }

    protected void OnTriggerEnter(Collider other)
    {
        var playerInfo = other.GetComponent<PlayerData>();
        if (playerInfo != null)
        {
            if (!players.Contains(playerInfo))
            {
                players.Add(playerInfo);
            }
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        var playerInfo = other.GetComponent<PlayerData>();
        if (playerInfo != null)
        {
            playerInfo.GetComponent<StatsController>().blindPower.Value = 300;
            players.Remove(playerInfo);
        }
    }

    public void ElympicsUpdate()
    {
        HandlePlayersInSandstorm();
        deathTimer.Value += Elympics.TickDuration;
        if (deathTimer.Value >= lifeTime)
        {
            foreach (var v in players)
            {
                v.GetComponent<StatsController>().blindPower.Value = 300;
            }
            ElympicsDestroy(gameObject);
        }
    }

    private void HandlePlayersInSandstorm()
    {
        foreach (var data in players)
        {
            damageTimer.Value += Elympics.TickDuration;
            if (damageTimer.Value >= 1f)
            {
                data.DealDamage(damage,owner);
                damageTimer.Value = 0f;
            }
            Vector3 distancetoCenter = transform.position - data.GetComponent<Rigidbody>().position;
            float accDistance = new Vector3(distancetoCenter.x, 0.0f, distancetoCenter.z).magnitude;
            data.GetComponent<StatsController>().blindPower.Value = accDistance;
            if (deathTimer.Value >= lifeTime)
            {
                data.GetComponent<StatsController>().blindPower.Value = 300;
            }
        }
        
    }

    public void setLifeTime(float value)
    {
        this.lifeTime.Value = value;
    }
}


