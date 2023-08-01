using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Elympics;

public class PlayerData : ElympicsMonoBehaviour, IObservable, IInitializable, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private int playerId = 0;
    [SerializeField] public string playerName = "Player";
    [SerializeField] private PlayerProvider playerProvider = null;
    [SerializeField] private StatsController statsController = null;

    public int PlayerId => playerId;
    
    public ElympicsInt Score { get; } = new ElympicsInt();

    public void Initialize()
    {

    }

    public void DealDamage(float damage, int damageOwner)
    {
        statsController.ChangeHealth(damage, damageOwner);
    }

    private void AddScore(int points)
    {
        Score.Value += points;
    }

    public void ProcessScore(int damageOwner)
    {
        if (damageOwner == PlayerId)
        {
            Score.Value -= 1;
        }
        else
        {
            playerProvider.GetPlayerById(damageOwner).AddScore(1);
        }
    }
    
    public void ElympicsUpdate()
    {
       
    }

}
