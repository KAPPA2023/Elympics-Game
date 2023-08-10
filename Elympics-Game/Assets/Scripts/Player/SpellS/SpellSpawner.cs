using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class SpellSpawner : ElympicsMonoBehaviour
{
    [SerializeField] private List<GameObject> spellList;          // TODO zmienic i podstawic gameobjecty spelli
    public Action SpellHit = null;

    public void TrySpawningSpell(Spells selectedSpell, Vector3 forward, int caster_id)
    {
        GameObject spellObject = spellList[(int)selectedSpell + 1];
        Spell spawnedSpell =  ElympicsInstantiate(spellObject.name,ElympicsPlayer.FromIndex(caster_id)).GetComponent<Spell>();
        spawnedSpell.SpawnSpell(transform.position, forward, caster_id);
        spawnedSpell.SetSpellHitCallback(SpellHit);
    }
}
