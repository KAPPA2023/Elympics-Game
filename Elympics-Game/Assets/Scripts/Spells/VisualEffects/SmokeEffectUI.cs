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
            Vector3 distancetoCenter = gameObject.transform.position - other.transform.position;
            float xDistance = Mathf.Abs(distancetoCenter.x);
            float zDistance = Mathf.Abs(distancetoCenter.z);

            float accDistance = xDistance > zDistance ? xDistance : zDistance;

            if (accDistance < 2f)
            {
                Color overlay = new Color(0.6f,0.6f,0.6f, 2f-accDistance);
                setOverlayColor(overlay,other);
            
                if (accDistance < 0.5f)
                {
                    Color fullOverlay = new Color(0.6f,0.6f,0.6f, 1);
                    setOverlayColor(fullOverlay,other);
            
                }
                
            }
        }
        
    }
    private void setOverlayColor(Color Color,Collider playerColiter)
    {
        Image smokeEffectsOverlay = playerColiter.GetComponentInChildren<Image>();
        smokeEffectsOverlay.color = Color;
    }

    private void OnTriggerExit(Collider other)
    {
        var playerInfo = other.GetComponent<PlayerData>();
        if (playerInfo != null)
        {
            Color resetColor = new Color(0, 0, 0, 0);
            setOverlayColor(resetColor,other);
        }
    }
}
