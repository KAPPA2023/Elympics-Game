using System;
using UnityEngine;
using Elympics;

public class PlayerData : ElympicsMonoBehaviour
{
    [Header("Parameters:")]
    [SerializeField] private int playerId = 0;
    [SerializeField] public string playerName = "Player";
    [SerializeField] private PlayerProvider playerProvider = null;
    [SerializeField] private StatsController statsController = null;
    
    public int PlayerId => playerId;
    public ElympicsInt Score { get; } = new ElympicsInt();
    public void ApplyModifier(int id, int spell)
    {
        switch (id)
        {
            case 0:
                statsController.SetMaxHealth(1);
                break;

            case 1:
                GetComponent<MovementController>().jumpForce = 40; 
                break;

            case 2:
                GetComponent<ActionHandler>().modified = spell;
                break;
        }
    }

    public void SetMagician()
    {
        GetComponent<ActionHandler>().theMagician = true;
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
}
