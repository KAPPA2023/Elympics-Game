using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using Unity.VisualScripting;
using UnityEngine;

public class SandStorm : ElympicsMonoBehaviour, IUpdatable
{
    private int owner;
    [SerializeField] protected float lifeTime;
    private List<PlayerData> players = new List<PlayerData>();
    private ElympicsFloat deathTimer = new ElympicsFloat(0.0f);
    private ElympicsFloat damageTimer = new ElympicsFloat(0.0f);
    // Start is called before the first frame update
  
    public virtual void SpawnSpell(Vector3 position,int client)
    {
        //we can use tick to setup timers - for example fireball could explode after 2 seconds in air
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

    protected virtual void OnTriggerStay(Collider other)
    {
        
            //SpellHit?.Invoke();
            foreach (var data in players)
            {
                damageTimer.Value += Elympics.TickDuration;
                if (damageTimer >= 1f)
                {
                    data.DealDamage(5,owner);
                    damageTimer.Value = 0f;
                }
                Vector3 distancetoCenter = gameObject.transform.position - other.transform.position;
                float xDistance = Mathf.Abs(distancetoCenter.x);
                float zDistance = Mathf.Abs(distancetoCenter.z);
                float accDistance = xDistance > zDistance ? xDistance : zDistance;
                data.GetComponent<StatsController>().blindPower.Value = accDistance;
                if (deathTimer >= lifeTime)
                {
                    data.GetComponent<StatsController>().blindPower.Value = 300;
                }
            }

            

    }
    
    public void ElympicsUpdate()
    {
        deathTimer.Value += Elympics.TickDuration;
        if (deathTimer >= lifeTime)
        {
            ElympicsDestroy(gameObject);
            Debug.Log(players.Count);
            foreach (var v in players)
            {
                v.GetComponent<StatsController>().blindPower.Value = 0;
            }
            
        }
    }
}
