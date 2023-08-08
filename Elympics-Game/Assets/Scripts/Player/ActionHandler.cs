using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
public enum Spells
{
    Empty = -1,
    Fireball = 0,
    Lightbolt = 1
}
public class ActionHandler : ElympicsMonoBehaviour, IUpdatable, IInitializable
{
    [SerializeField] private SpellSpawner spellSpawner;
    [SerializeField] private GameObject viewController;
    [SerializeField] private float spellCooldown = 0.8f;

    protected ElympicsFloat currentTimeBetweenShoots = new ElympicsFloat();
    protected bool canCast => currentTimeBetweenShoots >= spellCooldown;
    
    private Spells _selectedSpell = Spells.Empty;
    private int _remainingUses = 0;
    private Spells[] _stashedSpells;
    
    public void Initialize()
    {
        _stashedSpells = new Spells[]{Spells.Empty, Spells.Empty, Spells.Empty};
    }
    
    public Spells[] getSpells()
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
        if (_selectedSpell != Spells.Empty && _remainingUses > 0)
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
        spellSpawner.TrySpawningSpell(Spells.Empty, direction, GetComponent<PlayerData>().PlayerId);
    }

    public void chooseSpell(Spells drawnSpell)
    {
        if (drawnSpell != Spells.Empty)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_stashedSpells[i] == drawnSpell)
                {
                    _selectedSpell = drawnSpell;
                    _stashedSpells[i] = Spells.Empty;
                    _remainingUses = 3;
                    break;
                }
            }
        }
    }
    public bool addSpell(Spells spellType)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_stashedSpells[i] == Spells.Empty)
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
