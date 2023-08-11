using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Elympics;
using UnityEngine;
using UnityEngine.UI;

public class SmokeEffectUI : ElympicsMonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        
        var playerInfo = other.GetComponent<PlayerData>();
        if (playerInfo != null)
        {
            

            
        }
        
    }
    private void setOverlayColor(Color Color,Collider playerColiter)
    {
        Image smokeEffectsOverlay = playerColiter.GetComponentInChildren<Image>();
        smokeEffectsOverlay.color = Color;
    }

    private void OnTriggerExit(Collider other)
    {
       
    }
}
