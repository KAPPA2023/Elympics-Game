using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIDamage : MonoBehaviour
{
    [SerializeField] private GameObject hitmark;
    [SerializeField] private GameObject deathScreen;
    
    [SerializeField] private PlayerProvider playerProvider;

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
}
