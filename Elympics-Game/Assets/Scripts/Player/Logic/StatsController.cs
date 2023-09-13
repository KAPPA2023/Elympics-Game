using Elympics;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class StatsController : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private float maxHealth = 100.0f;
    public ElympicsFloat _health = new ElympicsFloat(0);
    public event Action<float, float> HealthValueChanged = null;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private DeathController deathController;

    #region Player states
    public ElympicsFloat blindPower = new ElympicsFloat(0f);
    public ElympicsBool isBlind = new ElympicsBool(false);
    public ElympicsFloat blindValue = new ElympicsFloat(0f);

    public float blindTime;
    private ElympicsFloat _blindTimer = new ElympicsFloat(0f);
    private ElympicsFloat _burningTimer = new ElympicsFloat(0f);
    #endregion
    
    public void Initialize()
    {
        _health.Value = maxHealth;
        _health.ValueChanged += OnHealthValueChanged;
    }

    public void ResetPlayerStats()
    {
        StopAllCoroutines();
        _blindTimer.Value = 0f;
        isBlind.Value = false;
        _health.Value = maxHealth;
    }

    public void ChangeHealth(float value, int damageOwner)
    {
        if (!Elympics.IsServer || deathController.IsDead.Value)
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

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        _health.Value = maxHealth;
    }

    public bool IsFullHp()
    {
        return Math.Abs(_health.Value - maxHealth) < 0.1;
    }

    
    
    public void ElympicsUpdate()
    {
        if (_burningTimer.Value > 0f)
        {
            _burningTimer.Value -= Elympics.TickDuration;
            if (_burningTimer.Value <= 0f)
            {
                StopAllCoroutines();
            }
        }
        
        if (isBlind.Value)
        {
            _blindTimer.Value += Elympics.TickDuration;
            if (_blindTimer.Value >= blindTime)
            {
                isBlind.Value = false;
                _blindTimer.Value = 0.0f;
            }
        }
    }

    #region Burning
    public void InitializeFire(int caster, float duration, float damagePerSecond)
    {
        if (_burningTimer.Value <= 0f)
        {
            _burningTimer.Value = duration;
            StartCoroutine(PlayerBurning(caster, damagePerSecond));
        }
        else
        {
            StopAllCoroutines();
            _burningTimer.Value = duration;
            StartCoroutine(PlayerBurning(caster, damagePerSecond));
        }
    }

    IEnumerator PlayerBurning(int caster, float damagePerSecond)
    {
        for (;;)
        {
            ChangeHealth(damagePerSecond / 5, caster);
            yield return new WaitForSeconds(0.2f);
        }
    }
    #endregion
}
