using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class SpellSpawner : ElympicsMonoBehaviour
{
    [SerializeField] private List<GameObject> spellList;
    private Action SpellHit = null;
    public Action RegisteredHit = null;
    private AudioSource _audioSource;

    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        SpellHit += HitRegistered;
    }
    
    [ElympicsRpc(ElympicsRpcDirection.ServerToPlayers)]  // specifying the direction
    private void HitRegistered()
    {
        // the contents of this method will be run on client instances
        RegisteredHit?.Invoke();
    }

    public void TrySpawningSpell(Spells selectedSpell, Vector3 forward, int caster_id, bool modified)
    {
        _audioSource.PlayOneShot(_audioSource.clip);
        GameObject spellObject = spellList[(int)selectedSpell + 1];
        Spell spawnedSpell =  ElympicsInstantiate("Spells/" + spellObject.name,ElympicsPlayer.FromIndex(caster_id)).GetComponent<Spell>();
        spawnedSpell.SpawnSpell(transform.position, forward, caster_id, modified);
        spawnedSpell.SetSpellHitCallback(SpellHit);
    }
}
