using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class SpellSpawner : ElympicsMonoBehaviour
{
    [SerializeField] private List<GameObject> spellList;          // TODO zmienic i podstawic gameobjecty spelli
    [SerializeField] private int spellCooldown;

    public void TrySpawningSpell(string selectedSpell, Vector3 forward, int caster_id)
    {
        if (Elympics.IsServer)
        {
            Spell spawnedSpell =  ElympicsInstantiate(selectedSpell, ElympicsPlayer.World ).GetComponent<Spell>();
            spawnedSpell.spawnSpell(transform.position, forward, caster_id,Elympics.Tick);
        }
    }
}
