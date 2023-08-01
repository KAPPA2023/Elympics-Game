using Elympics;
using System;
using UnityEngine;

public class StatsController : ElympicsMonoBehaviour, IInitializable
{
    [Header("Parameters:")]
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private DeathController deathController;
    //[Header("References:")]
    //[SerializeField] private DeathController deathController = null;

    private ElympicsFloat health = new ElympicsFloat(0);
    public ElympicsBool isDead = new ElympicsBool(false);
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

        if (!(health.Value <= 0.0f)) return;
        isDead.Value = true;
        deathController.ProcessPlayersDeath(damageOwner);
    }

    private void OnHealthValueChanged(float lastValue, float newValue)
    {
        HealthValueChanged?.Invoke(newValue, maxHealth);
    }
}
