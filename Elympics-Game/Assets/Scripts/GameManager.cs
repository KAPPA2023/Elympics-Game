using Elympics;
using System;
using UnityEngine;

public class GameManager : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private float timeToStartMatch = 1.0f;
    [SerializeField] private float matchLength = 120.0f;

    public ElympicsBool gameEnded = new ElympicsBool(false);
    
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
                gameEnded.Value = true;
                if (Elympics.IsServer)
                {
                    Elympics.EndGame();
                }
            }
        }
    }
}
