using Elympics;
using System;
using UnityEngine;

public class GameManager : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private float matchLength = 120.0f;
    [SerializeField] private float postGameTime = 5.0f;

    public ElympicsBool gameEnded = new ElympicsBool(false);
    public ElympicsFloat CurrentTimeOfMatchRemaining { get; } = new ElympicsFloat(0.0f);
    public ElympicsFloat PostGameTime { get; } = new ElympicsFloat(0.0f);
    
    public void Initialize()
    {
        if (!Elympics.IsServer) return;
        CurrentTimeOfMatchRemaining.Value = matchLength;
        PostGameTime.Value = postGameTime;
    }
    
    public void ElympicsUpdate()
    {
            CurrentTimeOfMatchRemaining.Value -= Elympics.TickDuration;
            if (!(CurrentTimeOfMatchRemaining.Value < 0.0f)) return;
            gameEnded.Value = true;
            PostGameTime.Value -= Elympics.TickDuration;
                
            if (Elympics.IsServer && PostGameTime.Value < 0.0f)
            {
                Elympics.EndGame();
            }
    }
}
