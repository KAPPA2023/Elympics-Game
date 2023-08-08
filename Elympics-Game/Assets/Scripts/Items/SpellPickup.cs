using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using Unity.VisualScripting;
using IInitializable = Elympics.IInitializable;

public class SpellPickup : ElympicsMonoBehaviour, IUpdatable, IInitializable
{
    [SerializeField] private List<GameObject> orbs;
    [SerializeField] private int spellType = 0;
    [SerializeField] private float respawnTime = 5.0f;
    
    public ElympicsFloat timeToSpawn = new ElympicsFloat();
    private ElympicsBool empty = new ElympicsBool(false);
    
    public void Initialize()
    {
        orbs[spellType].SetActive(true);
        empty.ValueChanged += ChangeSpellPickup;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!Elympics.IsServer) return;

        if (other.GetComponent<ActionHandler>() != null)
        {
            if (other.GetComponent<ActionHandler>().addSpell((Spells)spellType))
            {
                timeToSpawn.Value = respawnTime;
                empty.Value = true;
            }
        }
    }
    public void ElympicsUpdate()
    {
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
