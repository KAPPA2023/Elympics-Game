using Elympics;
using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class StatsController : ElympicsMonoBehaviour, IInitializable
{
    [Header("Parameters:")]
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private DeathController deathController;
    public ElympicsFloat BlindPower= new ElympicsFloat(0.0f);
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

        if (!(health.Value <= 0.0f)) return;
        deathController.ProcessPlayersDeath(damageOwner);
    }

    private void OnHealthValueChanged(float lastValue, float newValue)
    {
        HealthValueChanged?.Invoke(newValue, maxHealth);
    }

    public bool isDead()
    {
        return deathController.getDead();
    }

    public void setMaxHealth(float value)
    {
        maxHealth = value;
        health.Value = maxHealth;
    }
}
