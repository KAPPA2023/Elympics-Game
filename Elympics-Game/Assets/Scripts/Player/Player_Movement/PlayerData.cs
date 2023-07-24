using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Elympics;

public class PlayerData : ElympicsMonoBehaviour, IObservable, IInitializable, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private int playerId = 0;
    [SerializeField] private PlayerProvider playerProvider = null;

    public int PlayerId => playerId;
    
    [Header("Parameters:")]
    [SerializeField] private float maxHealth = 100;
    private ElympicsFloat health = new ElympicsFloat();
    
    
    public ElympicsBool IsDead { get; } = new ElympicsBool(false);
    public ElympicsInt Score { get; } = new ElympicsInt();
    public ElympicsFloat CurrentDeathTime { get; } = new ElympicsFloat(0.0f);

    public void Initialize()
    {
        health.Value = maxHealth;
    }
    
    public void DealDamage(float damage, int damageOwner)
    {
        if (!Elympics.IsServer) return;
        
        health.Value -= damage;
        
        if (health.Value <= 0.0f) ProcessDeath(damageOwner);
        
    }

    private void AddScore(int points)
    {
        Score.Value += points;
    }

    public void ProcessDeath(int damageOwner)
    {
        if (damageOwner == PlayerId)
        {
            Score.Value -= 1;
        }
        else
        {
            playerProvider.GetPlayerById(damageOwner).AddScore(1);
        }

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        
        //TODO: replace this value in new system 
        CurrentDeathTime.Value = 1.0f;
        IsDead.Value = true;
    }
    
    public void ElympicsUpdate()
    {
        if (!IsDead || !Elympics.IsServer)
            return;
        
        CurrentDeathTime.Value -= Elympics.TickDuration;
        if (CurrentDeathTime.Value <= 0)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        PlayerSpawner.Instance.SpawnPlayer(this);
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        health.Value = maxHealth;
        IsDead.Value = false;
    }
}
