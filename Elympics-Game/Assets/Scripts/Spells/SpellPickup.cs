using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class SpellPickup : ElympicsMonoBehaviour, IUpdatable, IInitializable
{
    [SerializeField] private List<GameObject> orbs;
    [SerializeField] private int spellType = 0;
    [SerializeField] private float respawnTime = 5.0f;
    
    private ElympicsFloat timeToSpawn = new ElympicsFloat();
    private ElympicsBool empty = new ElympicsBool(false);
    
    public void Initialize()
    {
        if (!gameObject.activeInHierarchy) return;
        orbs[spellType].SetActive(true);
        empty.ValueChanged += ChangeSpellPickup;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!Elympics.IsServer) return;

        if (other.GetComponent<ActionHandler>() != null)
        {
            if (other.GetComponent<ActionHandler>().AddSpell((Spells)spellType))
            {
                timeToSpawn.Value = respawnTime;
                empty.Value = true;
            }
        }
    }
    public void ElympicsUpdate()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!empty.Value) return;
        if (timeToSpawn.Value > 0.0f)
        {
            timeToSpawn.Value -= Elympics.TickDuration;
        }
        else
        {
            empty.Value = false;
        }
    }

    private void ChangeSpellPickup(bool oldVal, bool newVal)
    {
        orbs[spellType].SetActive(!newVal);
        GetComponent<Collider>().enabled = !newVal;
    }
}
