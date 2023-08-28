using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class WaterSpell : Spell
{
   public ElympicsFloat blindValue = new ElympicsFloat(0.8f);
   protected override void OnTriggerEnter(Collider other)
   {
      var playerInfo = other.GetComponent<PlayerData>();
      if (playerInfo == null) return;
      if (playerInfo.PlayerId == caster) return;
      base.OnTriggerEnter(other);
      if (playerInfo != null)
      {
         playerInfo.GetComponent<StatsController>().isBlind.Value = true;
         playerInfo.GetComponent<StatsController>().blindValue = this.blindValue;
      }
   }

   public override void ApplyModifier()
   {
      base.ApplyModifier();
      blindValue.Value = 1f;
   }
}
