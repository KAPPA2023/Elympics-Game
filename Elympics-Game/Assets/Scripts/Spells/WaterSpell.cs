using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class WaterSpell : Spell
{
    [SerializeField] private float blindValue;
    [SerializeField] private float modifiedBlindValue;
    [SerializeField] private float effectTime;
    [SerializeField] private float weakenedJumpForce;
    [SerializeField] private float modifiedWeakenedJumpForce;
    protected override void OnTriggerEnter(Collider other)
    {
        var playerInfo = other.GetComponent<PlayerData>();
        if (playerInfo == null) return;
        if (playerInfo.PlayerId == caster) return;
        base.OnTriggerEnter(other);
        if (playerInfo != null)
        {
            playerInfo.GetComponent<StatsController>().blindTime = effectTime;
            playerInfo.GetComponent<StatsController>().isBlind.Value = true;
            playerInfo.GetComponent<StatsController>().blindValue.Value = blindValue;
        }

        var playerMC = other.GetComponent<MovementController>();
        playerMC.StartWeakenedJump(effectTime, weakenedJumpForce);
    }

    public override void ApplyModifier()
    {
        base.ApplyModifier();
        blindValue = modifiedBlindValue;
        weakenedJumpForce = modifiedWeakenedJumpForce;
    }

}
