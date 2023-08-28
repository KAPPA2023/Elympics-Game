using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class IceScript : Spell
{
    private ElympicsFloat slowValue = new ElympicsFloat(5);
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var playerData = other.GetComponent<PlayerData>();
        if (playerData == null) return;
        if (playerData.PlayerId == caster) return;
        playerData.GetComponent<MovementController>().Slow();
        playerData.GetComponent<MovementController>().slowValue.Value=this.slowValue.Value;
        
    }

    public override void ApplyModifier()
    {
        base.ApplyModifier();
        slowValue.Value = 10f;
    }
}
