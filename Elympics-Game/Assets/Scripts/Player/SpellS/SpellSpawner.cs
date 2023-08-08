using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class SpellSpawner : ElympicsMonoBehaviour
{
    [SerializeField] private List<GameObject> spellList;          // TODO zmienic i podstawic gameobjecty spelli
    [SerializeField] private int spellCooldown;

    public void TrySpawningSpell(Spells selectedSpell, Vector3 forward, int caster_id)
    {
        if (Elympics.IsServer)
        {
            GameObject spellObject = spellList[(int)selectedSpell + 1];
            Spell spawnedSpell =  ElympicsInstantiate(spellObject.name, ElympicsPlayer.World ).GetComponent<Spell>();
            spawnedSpell.spawnSpell(transform.position, forward, caster_id,Elympics.Tick);
        }
    }
}
