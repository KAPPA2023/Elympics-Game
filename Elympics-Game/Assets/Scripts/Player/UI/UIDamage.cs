using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIDamage : MonoBehaviour
{
    [SerializeField] private GameObject hitmark;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private PlayerProvider playerProvider;
    [SerializeField] private Image smokeEffectsOverlay;

    private float _remainingHitmarkTime = 0.0f;
    
    private void Start()
    {
        if (playerProvider.IsReady)
            SubscribeToStatsController();
        else
            playerProvider.IsReadyChanged += SubscribeToStatsController;
    }

    private void Update()
    {
       
        if (_remainingHitmarkTime > 0.0f)
        {
            _remainingHitmarkTime -= Time.deltaTime;
        }
        else if (hitmark.activeInHierarchy)
        {
            hitmark.SetActive(false);
        }
    }

    private void SubscribeToStatsController()
    {
        var clientPlayerData = playerProvider.ClientPlayer;
        clientPlayerData.GetComponent<DeathController>().IsDead.ValueChanged += OnDeath;
        clientPlayerData.GetComponentInChildren<SpellSpawner>().SpellHit += OnEnemyDamaged;
        clientPlayerData.GetComponent<StatsController>().blindPower.ValueChanged += OnBlind;
        clientPlayerData.GetComponent<StatsController>().isBlind.ValueChanged += WaterBlind;
    }
    
    private void OnEnemyDamaged()
    {
        hitmark.SetActive(true); 
        _remainingHitmarkTime = 0.2f;
    }
    private void OnDeath(bool oldVal, bool newVal)
    {
        deathScreen.SetActive(newVal);
    }

    private void OnBlind(float oldValue, float newValue)
    {
        Color  overlay = new Color(0f,0f,0f, 0);
        if (newValue < 2f)
        {
            overlay = new Color(0.6f,0.6f,0.6f, 2f-newValue);
            
            
            if (newValue < 0.5f )
            {
                overlay = new Color(0.6f,0.6f,0.6f, 1);
            }

            
        }
        smokeEffectsOverlay.color = overlay;
      
    }
    
    private void WaterBlind(bool lastvalue, bool newvalue)
    {
        var clientPlayerData = playerProvider.ClientPlayer;
        Color overlay;
        if (newvalue == true)
        {
              overlay = new Color(0f,0.6f,clientPlayerData.GetComponent<StatsController>().blindValue, clientPlayerData.GetComponent<StatsController>().blindValue);
        }
        else
        {
            overlay = new Color(0f,0f,0f, 0f);
        }
        
        smokeEffectsOverlay.color = overlay;
    }
    
    
}
