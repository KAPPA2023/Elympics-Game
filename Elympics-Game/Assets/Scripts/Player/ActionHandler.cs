using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class ActionHandler : MonoBehaviour, IObservable
{
    [SerializeField] private SpellSpawner spellSpawner;

    [SerializeField] private ElympicsInt currentAction = new ElympicsInt(); //0 - idle, 1- casting spell, 2 - drawing spell

    public int GetCurrentAction()
    {
        return currentAction.Value;
    }

    public void HandleActions(bool attack, bool draw, long tick)
    {
        int lastAction = currentAction.Value;

        if (lastAction == 0)
        {
            if (attack)
            {
                spellSpawner.TrySpawningSpell();
            }
        }
        else if (lastAction == 1)
        {
            currentAction.Value = 0;
        }
    }
}
