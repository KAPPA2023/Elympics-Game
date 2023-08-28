using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class IceScript : Spell
{
    [SerializeField] private float slowValue = 5.0f;
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var playerData = other.GetComponent<PlayerData>();
        if (playerData == null) return;
        if (playerData.PlayerId == caster) return;
        playerData.GetComponent<MovementController>().Slow();
        playerData.GetComponent<MovementController>().slowValue.Value = slowValue;
    }

    public override void ApplyModifier()
    {
        base.ApplyModifier();
        slowValue *= 2f;
    }
}
