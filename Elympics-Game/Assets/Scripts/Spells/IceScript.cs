using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class IceScript : Spell
{
    [SerializeField] private float slowValue = 0.5f;
    [SerializeField] private float slowDuration = 4f;
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var playerData = other.GetComponent<PlayerData>();
        if (playerData == null) return;
        if (playerData.PlayerId == caster) return;
        playerData.GetComponent<MovementController>().Slow(slowValue,slowDuration);
    }

    public override void ApplyModifier()
    {
        base.ApplyModifier();
        slowValue = 0.1f;
    }
}
