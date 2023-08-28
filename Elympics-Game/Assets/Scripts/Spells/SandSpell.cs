using System.Collections;
using System;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class SandSpell : Spell
{
    [SerializeField] private GameObject sandstorm;
    private ElympicsBool spawned = new ElympicsBool(false);
    private ElympicsBool isModified = new ElympicsBool(false);
    private ElympicsFloat LifeTime = new ElympicsFloat(4);
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
                SandStorm ss = ElympicsInstantiate(sandstorm.name, ElympicsPlayer.All).GetComponent<SandStorm>();
                ss.setLifeTime(lifeTime);
                ss.SpawnSpell(this.transform.position + new Vector3(0, 0.1f, 0), caster);
                spawned.Value = true;
            }
        }
    }

    public override void ApplyModifier()
    {
        base.ApplyModifier();
        this.LifeTime.Value = 8;
    }
}
