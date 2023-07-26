using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIDamage : MonoBehaviour
{
    //TODO: rework this class because fuszara fest tu jest zrobiona
    [SerializeField] private GameObject hitmark;
    [SerializeField] private GameObject deathScreen;
    
    [SerializeField] private PlayerProvider playerProvider;

    private float remainingHitmarkTime = 0.0f;
    
    private void Start()
    {
        if (playerProvider.IsReady)
            SubscribeToStatsController();
        else
            playerProvider.IsReadyChanged += SubscribeToStatsController;
    }

    private void Update()
    {
        if (remainingHitmarkTime > 0.0f)
        {
            remainingHitmarkTime -= Time.deltaTime;
        }
        else if (hitmark.activeInHierarchy)
        {
            hitmark.SetActive(false);
        }
    }

    private void SubscribeToStatsController()
    {
        var clientPlayerData = playerProvider.ClientPlayer;
        var enemyPlayerData = playerProvider.AllPlayersInScene[1 - clientPlayerData.PlayerId];
        clientPlayerData.GetComponent<StatsController>().isDead.ValueChanged += OnDeath;
        enemyPlayerData.GetComponent<StatsController>().HealthValueChanged += OnEnemyDamaged;
    }
    
    public void OnEnemyDamaged(float oldVal, float newVal)
    {
        if (oldVal < 100.0f)
        {
            hitmark.SetActive(true);
            remainingHitmarkTime = 0.2f;
        }
    }
    private void OnDeath(bool oldVal, bool newVal)
    {
        deathScreen.SetActive(newVal);
    }
}
