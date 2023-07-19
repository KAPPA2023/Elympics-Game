using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class SpellSpawner : ElympicsMonoBehaviour
{
    [SerializeField] private GameObject spellObject;
    [SerializeField] private int spellCooldown;

    public void TrySpawningSpell()
    {
        if (Elympics.IsServer)
        {
            TestSpellHandler spawnedSpell =  ElympicsInstantiate(spellObject.name, ElympicsPlayer.World).GetComponent<TestSpellHandler>();
            spawnedSpell.spawnSpell(transform.position, transform.forward, Elympics.Tick);
        }
    }
}
