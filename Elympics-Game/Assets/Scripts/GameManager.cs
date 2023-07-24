using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private float timeToStartMatch = 1.0f;
    [SerializeField] private float matchLength = 120.0f;
    [SerializeField] private PlayerProvider playerProvider = null;
    private bool gameEnded = false;
    
    public ElympicsFloat CurrentTimeToStartMatch { get; } = new ElympicsFloat(0.0f);
    public ElympicsFloat CurrentTimeOfMatchRemaining { get; } = new ElympicsFloat(0.0f);
	
    private ElympicsBool gameInitializationEnabled = new ElympicsBool(false);
    
    public void Initialize()
    {
        if (Elympics.IsServer)
        {
            CurrentTimeToStartMatch.Value = timeToStartMatch;
            CurrentTimeOfMatchRemaining.Value = matchLength;
            gameInitializationEnabled.Value = true;
        }
    }
    
    public void ElympicsUpdate()
    {
        
        if (gameInitializationEnabled)
        {
            CurrentTimeToStartMatch.Value -= Elympics.TickDuration;
            if (CurrentTimeToStartMatch < 0.0f)
            {
                gameInitializationEnabled.Value = false;
            }
        }
        else
        {
            CurrentTimeOfMatchRemaining.Value -= Elympics.TickDuration;
            if (CurrentTimeOfMatchRemaining.Value < 0.0f)
            {
                if (Elympics.IsServer)
                {
                    
                    Elympics.EndGame();
                }
            }
        }
    }

    private void GetWinningPlayer()
    {
        PlayerData[] players = playerProvider.AllPlayersInScene;

        int highest_score = players[0].Score;

    }

}
