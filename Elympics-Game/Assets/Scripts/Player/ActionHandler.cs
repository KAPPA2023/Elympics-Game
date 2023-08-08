using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEngine.Serialization;

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
    public ElympicsArray<ElympicsInt> stashedSpells = new ElympicsArray<ElympicsInt>(3, () => new ElympicsInt(-1));
    public event Action<int, int> stashedSpellChanged = null;

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
                if (stashedSpells.Values[i].Value == (int)drawnSpell)
                {
                    _selectedSpell = drawnSpell;
                    stashedSpells.Values[i].Value = (int)Spells.Empty;
                    _remainingUses = GetSpellUses(drawnSpell);
                    break;
                }
            }
        }
    }
    public bool addSpell(Spells spellType)
    {
        for (int i = 0; i < 3; i++)
        {
            if (stashedSpells.Values[i].Value == (int)Spells.Empty)
            {
                stashedSpells.Values[i].Value = (int)spellType;
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
    private int GetSpellUses(Spells spell)
    {
        switch (spell)
        {
            case Spells.Empty: return 0;
            case Spells.Fireball: return 3;
            case Spells.Lightbolt: return 2;
            default: return 0;
        }
    }
    
    public void Initialize()
    {
        //Tried for(i = 0; i <3; i++) but it didn't work lul if you know why fix it ;)
        stashedSpells.Values[0].ValueChanged += delegate(int value, int newValue)
        {
            stashedSpellChanged.Invoke(0, stashedSpells.Values[0].Value);
        };
        stashedSpells.Values[1].ValueChanged += delegate(int value, int newValue)
        {
            stashedSpellChanged.Invoke(1, stashedSpells.Values[1].Value);
        };
        stashedSpells.Values[2].ValueChanged += delegate(int value, int newValue)
        {
            stashedSpellChanged.Invoke(2, stashedSpells.Values[2].Value);
        };
    }
}