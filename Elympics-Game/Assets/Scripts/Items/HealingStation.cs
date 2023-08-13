using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class HealingStation : ElympicsMonoBehaviour, IUpdatable, IInitializable
{
    
    [SerializeField] private GameObject orb;
    [SerializeField] private float respawnTime = 5.0f;
    [SerializeField] private float healingValue = 50.0f;
    
    private ElympicsFloat timeToSpawn = new ElympicsFloat();
    private ElympicsBool _empty = new ElympicsBool(false);
    
    public void Initialize()
    {
        orb.SetActive(true);
        _empty.ValueChanged += ChangeHealPickup;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!Elympics.IsServer) return;
        var player = other.GetComponent<StatsController>();
        if (player != null)
        {
            if (!player.IsFullHp())
            {
                player.ChangeHealth(-healingValue,player.GetComponent<PlayerData>().PlayerId);
                timeToSpawn.Value = respawnTime; 
                _empty.Value = true;
            }
        }
    }
    public void ElympicsUpdate()
    {
        if (!_empty.Value) return;
        if (timeToSpawn.Value > 0.0f)
        {
            timeToSpawn.Value -= Elympics.TickDuration;
        }
        else
        {
            _empty.Value = false;
        }
    }

    private void ChangeHealPickup(bool oldVal, bool newVal)
    {
        orb.SetActive(!newVal);
        GetComponent<Collider>().enabled = !newVal;
    }
    
}
