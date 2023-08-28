using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class Fireball : Spell
{
    [SerializeField] private GameObject fireExplosion;
    private ElympicsBool spawned = new ElympicsBool(false);
    private ElympicsFloat burnTime = new ElympicsFloat(3f);

    protected override void SpawnChild()
    {
        if (!spawned.Value)
        {
            if (Elympics.IsServer)
            {
                FireExplosion ss = ElympicsInstantiate(fireExplosion.name, ElympicsPlayer.World).GetComponent<FireExplosion>();
                ss.SpawnSpell(this.transform.position + new Vector3(0, 0.1f, 0), caster);
            }
            spawned.Value = true;
        }
    }

    public override void ApplyModifier()
    {
        base.ApplyModifier();
        burnTime.Value = 6f;
    }
}
