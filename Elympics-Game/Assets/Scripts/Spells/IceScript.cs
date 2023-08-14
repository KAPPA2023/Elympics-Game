using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScript : Spell
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var playerData = other.GetComponent<PlayerData>();
        if (playerData == null) return;
        if (playerData.PlayerId == caster) return;
        playerData.GetComponent<MovementController>().Slow();
    }
}
