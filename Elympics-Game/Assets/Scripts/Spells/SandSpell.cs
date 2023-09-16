using System.Collections;
using System;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class SandSpell : Spell
{
    [SerializeField] private float smokeLifeTime;
    [SerializeField] private GameObject sandstorm;
    private ElympicsBool spawned = new ElympicsBool(false);
    protected override void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var playerInfo = collision.gameObject.GetComponent<PlayerData>();
        if (playerInfo != null)
        {
            if (playerInfo.PlayerId == caster) return;
            SpellHit?.Invoke();
            playerInfo.DealDamage(spellDamage, caster);
            DetonateProjectile();
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            DetonateProjectile();
        }
    }

    protected override void SpawnChild()
    {
        if (!spawned.Value)
        {
            if (Elympics.IsServer)
            {
                SandStorm ss = ElympicsInstantiate("Spells/" + sandstorm.name, ElympicsPlayer.All).GetComponent<SandStorm>();
                ss.SpawnSpell(this.transform.position + new Vector3(0, 0.1f, 0), caster);
                ss.deathTimer.Value = smokeLifeTime;
                spawned.Value = true;
            }
        }
    }

    public override void ApplyModifier()
    {
        base.ApplyModifier();
        this.smokeLifeTime = smokeLifeTime * 2f;
    }
}
