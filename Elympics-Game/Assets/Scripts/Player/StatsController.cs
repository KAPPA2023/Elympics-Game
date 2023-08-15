using Elympics;
using System;
using UnityEngine;

public class StatsController : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private DeathController deathController;
    public ElympicsFloat blindPower= new ElympicsFloat(0.0f);
    public ElympicsBool isBlind = new ElympicsBool(false);
    private ElympicsFloat _blindTimer = new ElympicsFloat(0.0f);

    public ElympicsBool isFire = new ElympicsBool(false);
    public ElympicsFloat _fireTimer = new ElympicsFloat(0.0f);
    //[Header("References:")]
    //[SerializeField] private DeathController deathController = null;

    private ElympicsFloat _health = new ElympicsFloat(0);
    public event Action<float, float> HealthValueChanged = null;
   
    public void Initialize()
    {
        _health.Value = maxHealth;
        _health.ValueChanged += OnHealthValueChanged;

        //deathController.PlayerRespawned += ResetPlayerStats;
    }

    public void ResetPlayerStats()
    {
        _health.Value = maxHealth;
    }

    public void ChangeHealth(float value, int damageOwner)
    {
        if (!Elympics.IsServer)
            return;

        _health.Value -= value;
        if (_health.Value > maxHealth) _health.Value = maxHealth;

        if (!(_health.Value <= 0.0f)) return;
        deathController.ProcessPlayersDeath(damageOwner);
    }

    private void OnHealthValueChanged(float lastValue, float newValue)
    {
        HealthValueChanged?.Invoke(newValue, maxHealth);
    }

    public bool IsDead()
    {
        return deathController.getDead();
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        _health.Value = maxHealth;
    }

    public bool IsFullHp()
    {
        return Math.Abs(_health.Value - maxHealth) < 0.1;
    }

    public void InitializeFire()
    {
        isFire.Value = true;
        _fireTimer.Value = 0f;
    }
    
    public void ElympicsUpdate()
    {

        if (isFire)
        {
            _health.Value -= 5 * Elympics.TickDuration;
            _fireTimer.Value += Elympics.TickDuration;
            if (_fireTimer >= 5)
            {
                isFire.Value = false;
                _fireTimer.Value = 0.0f;
            }
        }
        
        if (isBlind)
        {
            _blindTimer.Value += Elympics.TickDuration;
            if (_blindTimer >= 2f)
            {
                isBlind.Value = false;
                _blindTimer.Value = 0.0f;
            }
        }
    }
}
