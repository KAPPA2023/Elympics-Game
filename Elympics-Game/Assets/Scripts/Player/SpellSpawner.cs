using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class SpellSpawner : ElympicsMonoBehaviour
{
    [SerializeField] private List<GameObject> spellList;
    [SerializeField] private int spellCooldown;

    public void TrySpawningSpell(int selectedSpell, Vector3 forward)
    {
        if (Elympics.IsServer)
        {
            GameObject spellObject = spellList[selectedSpell];
            Spell spawnedSpell =  ElympicsInstantiate(spellObject.name, ElympicsPlayer.World ).GetComponent<Spell>();
            spawnedSpell.spawnSpell(transform.position, forward, Elympics.Tick);
        }
    }
}
