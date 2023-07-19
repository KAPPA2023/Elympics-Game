using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class ActionHandler : MonoBehaviour, IObservable
{
    [SerializeField] private SpellSpawner spellSpawner;

    [SerializeField] private ElympicsInt currentAction = new ElympicsInt(); //0 - idle, 1- casting spell, 2 - drawing spell
    
    private int _selectedSpell = -1;
    private int _remainingUses = 0;
    
    public void HandleActions(bool attack, bool draw, int shape , long tick)
    {
        int lastAction = currentAction.Value;
        chooseSpell(shape);
        if (lastAction == 0)
        {
            if (attack)
            {
                castSpell();
            }
        }
        else if (lastAction == 1)
        {
            currentAction.Value = 0;
        }
    }
    
    public void castSpell()
    {
        if (_selectedSpell != -1 && _remainingUses > 0)
        {
            spellSpawner.TrySpawningSpell(_selectedSpell + 1);
            _remainingUses--;
        }
        else
        {
            castBasicAttack();
        }
    }

    public void castBasicAttack()
    {
        spellSpawner.TrySpawningSpell(0);
    }

    public void chooseSpell(int drawn_spell)
    {
        if (drawn_spell >= 0)
        {
            
            _selectedSpell = drawn_spell;
            isInInventory(drawn_spell);
        }
    }

    private void isInInventory(int drawn_spell)
    {
        switch (drawn_spell)
        {
            case 0:
                if (this.gameObject.GetComponent<Inventory>().canCastFireball())
                {
                    _remainingUses = 3;
                }
                else
                {
                    _remainingUses = 0;
                    _selectedSpell = -1;
                }

                break;

            case 1:
                if(this.gameObject.GetComponent<Inventory>().canCastLightning())
                {_remainingUses = 1;}
            
                else
                {
                    _remainingUses = 0;
                    _selectedSpell = -1;
                }

                break;
        }
    }
    
    public int GetCurrentAction()
    {
        return currentAction.Value;
    }
}
