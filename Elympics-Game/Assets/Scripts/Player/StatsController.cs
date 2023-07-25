using Elympics;
using System;
using UnityEngine;

public class StatsController : ElympicsMonoBehaviour, IInitializable
{
    [Header("Parameters:")]
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private bool isDead = false;
    [SerializeField] private PlayerData playerData;
    //[Header("References:")]
    //[SerializeField] private DeathController deathController = null;

    private ElympicsFloat health = new ElympicsFloat(0);
    public event Action<float, float> HealthValueChanged = null;

    public void Initialize()
    {
        health.Value = maxHealth;
        health.ValueChanged += OnHealthValueChanged;

        //deathController.PlayerRespawned += ResetPlayerStats;
    }

    public void ResetPlayerStats()
    {
        health.Value = maxHealth;
    }

    public void ChangeHealth(float value, int damageOwner)
    {
        if (!Elympics.IsServer)
            return;

        health.Value -= value;

        if (health.Value <= 0.0f)
            playerData.ProcessDeath(damageOwner);
    }

    private void OnHealthValueChanged(float lastValue, float newValue)
    {
        HealthValueChanged?.Invoke(newValue, maxHealth);
    }
}
