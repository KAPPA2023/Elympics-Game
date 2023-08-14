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
    Lightbolt = 1,
    WaterBlast = 2,
    SandGranade = 3,
    Tornado = 4,
    IceSpike = 5
}
public class ActionHandler : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private SpellSpawner spellSpawner;
    [SerializeField] private GameObject viewController;
    [SerializeField] private float spellCooldown = 0.8f;
    public bool modified = false;
    public delegate void MyEventHandler();
    public static event MyEventHandler OnBadSpell;
    public static event MyEventHandler OnGoodSpell;


    protected ElympicsFloat currentTimeBetweenShoots = new ElympicsFloat();
    protected bool canCast => currentTimeBetweenShoots >= spellCooldown;
    
    private Spells _selectedSpell = Spells.Empty;
    public ElympicsInt _remainingUses = new ElympicsInt(0);
    public ElympicsArray<ElympicsInt> stashedSpells = new ElympicsArray<ElympicsInt>(3, () => new ElympicsInt(-1));
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
            spellSpawner.TrySpawningSpell(_selectedSpell,direction, GetComponent<PlayerData>().PlayerId, modified);
            _remainingUses.Value--;
        }
        else 
        {
            castBasicAttack(direction);
        }

        currentTimeBetweenShoots.Value = 0.0f;
    }

    public void castBasicAttack(Vector3 direction)
    {
        spellSpawner.TrySpawningSpell(Spells.Empty, direction, GetComponent<PlayerData>().PlayerId, false);
    }

    public void chooseSpell(Spells drawnSpell)
    {
        if (drawnSpell != Spells.Empty)
        {
            for (int i = 0; i < 3; i++)
            {
                if (stashedSpells.Values[i].Value == (int)drawnSpell)
                {
                    Invoke("GoodSpellInvoke", 0.1f);
                    _selectedSpell = drawnSpell;
                    stashedSpells.Values[i].Value = (int)Spells.Empty;
                    _remainingUses.Value = GetSpellUses(drawnSpell);
                    break;
                }
                else
                {
                    Invoke("BadSpellInvoke", 0.1f);
                }
            }
        }

    }
    public bool addSpell(Spells spellType)
    {
        Debug.Log(spellType);
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(stashedSpells.Values[i].Value);
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
            case Spells.SandGranade: return 2;
            case Spells.WaterBlast: return 2;
            case Spells.Tornado: return 2;
            case Spells.IceSpike: return 2;
            default: return 0;
        }
    }

    public int getRemainingUses()
    {
        return _remainingUses;
    }

    private void GoodSpellInvoke()
    {
        OnGoodSpell();
    }
    private void BadSpellInvoke()
    {
        OnBadSpell();
    }
}