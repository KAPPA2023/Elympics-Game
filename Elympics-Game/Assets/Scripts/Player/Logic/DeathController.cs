using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DeathController : ElympicsMonoBehaviour, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private float deathTime = 2.0f;

    public ElympicsBool IsDead { get; } = new ElympicsBool(false);
    public ElympicsFloat CurrentDeathTime { get; } = new ElympicsFloat(0.0f);

    public event Action PlayerRespawned = null;
    public event Action<int, int> HasBeenKilled = null;


    private PlayerData playerData = null;
    private StatsController playerStats = null;

    private void Awake()
    {
        playerData = GetComponent<PlayerData>();
        playerStats = GetComponent<StatsController>();
    }

    public void ProcessPlayersDeath(int damageOwner)
    {
        playerData.ProcessScore(damageOwner);

        CurrentDeathTime.Value = deathTime;
        IsDead.Value = true;

        Debug.Log("dead");

        HasBeenKilled?.Invoke((int)PredictableFor, damageOwner);
    }

    public void ElympicsUpdate()
    {
        if (!IsDead || !Elympics.IsServer)
            return;

        CurrentDeathTime.Value -= Elympics.TickDuration;

        if (CurrentDeathTime.Value <= 0)
        {
            //Respawn player
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        PlayerSpawner.Instance.SpawnPlayer(playerData);
        IsDead.Value = false;
        ResetPlayerStats();
    }

    private void ResetPlayerStats()
    {
        playerStats.ResetPlayerStats();
    }

    public bool getDead()
    {
        return IsDead.Value;
    }

}
