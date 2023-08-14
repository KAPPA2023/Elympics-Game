using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class WaterSpell : Spell
{
   
   
   protected override void OnTriggerEnter(Collider other)
   {
      var playerInfo = other.GetComponent<PlayerData>();
      if (playerInfo.PlayerId != caster)
      {
         base.OnTriggerEnter(other);
         if (playerInfo != null)
         {
            playerInfo.GetComponent<StatsController>().isBlind.Value = true;
         } 
      }
      
      
   }
   
}
