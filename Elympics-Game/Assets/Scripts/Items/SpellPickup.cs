using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using Unity.VisualScripting;

public class SpellPickup : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] protected int spellID = -1;
    [SerializeField] private float respawnTime = 5.0f;
    public ElympicsFloat timeToSpawn = new ElympicsFloat();
    private ElympicsBool empty = new ElympicsBool(false);
    public void Awake()
    {
        Color color = (spellID == 0) ? Color.red : Color.yellow;
        GetComponentInChildren<Renderer>().material.SetColor("_Color",color);
        empty.ValueChanged += ChangeSpellPickup;
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: this should be reworked as well as ActionHandler.addSpell, collected spells should be synchronised and controlled by server
        if (other.GetComponent<ActionHandler>() != null)
        {
            if (other.GetComponent<ActionHandler>().addSpell(spellID))
            {
                timeToSpawn.Value = respawnTime;
                empty.Value = true;
                GetComponent<Collider>().enabled = false;
                GetComponentInChildren<Renderer>().material.SetColor("_Color",Color.black);
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
        if (!newVal)
        {
            GetComponent<Collider>().enabled = true;
            Color color = (spellID == 0) ? Color.red : Color.yellow;
            GetComponentInChildren<Renderer>().material.SetColor("_Color",color);
        }
    }
}
