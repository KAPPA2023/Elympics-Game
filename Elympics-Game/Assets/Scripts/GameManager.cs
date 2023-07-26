using Elympics;
using UnityEngine;

public class GameManager : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private float matchLength = 120.0f;
    [SerializeField] private float postGameTime = 5.0f;

    public ElympicsBool gameEnded = new ElympicsBool(false);
    public ElympicsFloat CurrentTimeOfMatchRemaining { get; } = new ElympicsFloat(0.0f);
    public ElympicsFloat PostGameTime { get; } = new ElympicsFloat(0.0f);
    public bool IsReady { get; private set; } = false;
    
    public void Initialize()
    {
        if (!Elympics.IsServer) return;
        CurrentTimeOfMatchRemaining.Value = matchLength;
        PostGameTime.Value = postGameTime;
        IsReady = true;
    }
    
    public void ElympicsUpdate()
    {
            CurrentTimeOfMatchRemaining.Value -= Elympics.TickDuration;
            if (!(CurrentTimeOfMatchRemaining.Value < 0.0f)) return;
            gameEnded.Value = true;
            PostGameTime.Value -= Elympics.TickDuration;
            if (PostGameTime.Value < 0.0f)
            {
                if (Elympics.IsServer)
                {
                    Elympics.EndGame();
                }
            }
    }
}
