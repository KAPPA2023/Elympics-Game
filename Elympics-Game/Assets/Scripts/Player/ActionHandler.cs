using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class ActionHandler : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private SpellSpawner spellSpawner;
    [SerializeField] private GameObject viewController;
    [SerializeField] private float spellCooldown = 0.8f;

    protected ElympicsFloat currentTimeBetweenShoots = new ElympicsFloat();
    protected bool canCast => currentTimeBetweenShoots >= spellCooldown;
    
    private string _selectedSpell = "empty";
    private int _remainingUses = 0;
    private string[] _stashedSpells;

    // ----------------------------------
    //              SPELLS
    // empty
    // fireBall
    // lightningBolt
    //
    // ----------------------------------

    public void Awake()
    {
        _stashedSpells = new string[]{"empty", "empty", "empty" };
    }

    public string[] getSpells()
    {
        return _stashedSpells;
    }

    public void HandleActions(bool attack)
    {
        if (attack && canCast)
        {
            castSpell(viewController.transform.forward);
        }
    }
    
    public void castSpell(Vector3 direction)
    {
        if (_selectedSpell != "empty" && _remainingUses > 0)
        {
            spellSpawner.TrySpawningSpell(_selectedSpell,direction, GetComponent<PlayerData>().PlayerId);
            _remainingUses--;
        }
        else 
        {
            castBasicAttack(direction);
        }

        currentTimeBetweenShoots.Value = 0.0f;
    }

    public void castBasicAttack(Vector3 direction)
    {
        spellSpawner.TrySpawningSpell("BasicAttack", direction, GetComponent<PlayerData>().PlayerId);
    }

    public void chooseSpell(string drawnSpell)
    {
        if (drawnSpell != "empty")
        {
            for (int i = 0; i < 3; i++)
            {
                if (_stashedSpells[i] == drawnSpell)
                {
                    _selectedSpell = drawnSpell;
                    _stashedSpells[i] = "empty";
                    _remainingUses = 3;
                    break;
                }
            }
        }
    }
    public bool addSpell(string spellType)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_stashedSpells[i] == "empty")
            {
                _stashedSpells[i] = spellType;
                return true;
            }
        }
        return false;
    }

    public void ElympicsUpdate()
    {
        if (!canCast)
        {
            currentTimeBetweenShoots.Value += Elympics.TickDuration;
        }    
    }
}
