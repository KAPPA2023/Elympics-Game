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
    [SerializeField] private AudioSource audioSource;
    
    private ElympicsFloat timeToSpawn = new ElympicsFloat();
    private ElympicsBool empty = new ElympicsBool(false);
    private bool hidden = false;
    
    public void Initialize()
    {
        
        var startGameController = GameObject.Find("GameController").GetComponent<StartGameController>();
        
        if (startGameController != null)
        {
            var gameManager = GameObject.Find("GameController").GetComponent<GameManager>();
            gameManager.matchTime.ValueChanged += SetupPickups;
        }
        else
        {
            orbs[spellType].SetActive(true);
        }
        empty.ValueChanged += ChangeSpellPickup;
    }

    private void SpellsHidden()
    {
        var modifier = GameObject.Find("GameController").GetComponent<StartGameController>().spellsHidden;
        if (modifier)
        {
            hidden = true;
            foreach (var orb in orbs)
            {
                orb.SetActive(false);
            }
            orbs[6].SetActive(true);
        }
        else
        {
            orbs[spellType].SetActive(true);
        }
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
                audioSource.Play();
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
            if (empty.Value)
            {
                empty.Value = false;
            }
        }
    }

    private void SetupPickups(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            SpellsHidden();
        }
        
    }

    private void ChangeSpellPickup(bool oldVal, bool newVal)
    {
        if (hidden)
        {
            orbs[6].SetActive(!newVal);
        }
        else
        {
            orbs[spellType].SetActive(!newVal);
        }
        
        GetComponent<Collider>().enabled = !newVal;
    }
}
