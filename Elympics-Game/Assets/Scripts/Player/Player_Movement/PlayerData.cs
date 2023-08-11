using System;
using UnityEngine;
using Elympics;

public class PlayerData : ElympicsMonoBehaviour, IInitializable, IUpdatable
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

    public void ElympicsUpdate()
    {
    }

    public void ApplyModifier(int id)
    {
        switch (id)
        {
            case 0:
                statsController.setMaxHealth(1);
                break;

            case 1:
                GetComponent<MovementController>().jumpForce = 40; 
                break;

            case 2:
                GetComponent<ActionHandler>().modified = true;
                break;

            default:
                break;
        }
    }

    #region ScoreLogic
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
    #endregion

    public void DealDamage(float damage, int damageOwner)
    {
        statsController.ChangeHealth(damage, damageOwner);
    }

    public bool isDead()
    {
        return statsController.isDead();
    }
}