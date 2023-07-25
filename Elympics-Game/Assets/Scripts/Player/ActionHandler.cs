using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class ActionHandler : MonoBehaviour, IObservable
{
    [SerializeField] private SpellSpawner spellSpawner;
    [SerializeField] private GameObject viewController;

    [SerializeField] private ElympicsInt currentAction = new ElympicsInt(); //0 - idle, 1- casting spell, 2 - drawing spell
    
    private int _selectedSpell = -1;
    private int _remainingUses = 0;
    private int[] _stashedSpells;

    public void Awake()
    {
        _stashedSpells = new int[]{-1, -1, -1};
    }

    public int[] getSpells()
    {
        return _stashedSpells;
    }

    public void HandleActions(bool attack, bool draw, int shape ,long tick)
    {
        int lastAction = currentAction.Value;
        chooseSpell(shape);
        if (lastAction == 0)
        {
            if (attack)
            {
                castSpell(viewController.transform.forward);
            }
        }
        else if (lastAction == 1)
        {
            currentAction.Value = 0;
        }
    }
    
    public void castSpell(Vector3 direction)
    {
        if (_selectedSpell != -1 && _remainingUses > 0)
        {
            spellSpawner.TrySpawningSpell(_selectedSpell + 1,direction, GetComponent<PlayerData>().PlayerId);
            _remainingUses--;
        }
        else
        {
            castBasicAttack(direction);
        }
    }

    public void castBasicAttack(Vector3 direction)
    {
        spellSpawner.TrySpawningSpell(0, direction, GetComponent<PlayerData>().PlayerId);
    }

    public void chooseSpell(int drawn_spell)
    {
        if (drawn_spell >= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_stashedSpells[i] == drawn_spell)
                {
                    
                    _selectedSpell = drawn_spell;
                    Debug.Log(_selectedSpell);
                    _stashedSpells[i] = -1;
                    _remainingUses = 3;
                    break;
                }
            }
        }
    }
    public bool addSpell(int spellID)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_stashedSpells[i] == -1)
            {
                _stashedSpells[i] = spellID;
                return true;
            }
        }
        return false;
    }
}
