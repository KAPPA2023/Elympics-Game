using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class KillPlane : ElympicsMonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Elympics.IsServer)
        {
            var playerInfo = other.GetComponent<PlayerData>();
            if (playerInfo != null)
            {
                playerInfo.DealDamage(1000, playerInfo.PlayerId);
            }
        }
    }
}
