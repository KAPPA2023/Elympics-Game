using Elympics;
using UnityEngine;

public class GameManager : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [SerializeField] private PlayerProvider playerProvider;
    [Header("Parameters:")]
    [SerializeField] private float matchLength = 120.0f;
    [SerializeField] private float postGameTime = 5.0f;
    [SerializeField] private float preGameTime = 5.0f;
    private ElympicsBool appliedModifiers = new ElympicsBool(false);

    public ElympicsBool matchTime = new ElympicsBool(false);
    public ElympicsBool gameEnded = new ElympicsBool(false);
    public ElympicsFloat CurrentTimeOfMatchRemaining { get; } = new ElympicsFloat(0.0f);
    public ElympicsFloat CurrentTimeToStartMatch { get; } = new ElympicsFloat(0.0f);
    public ElympicsFloat PostGameTime { get; } = new ElympicsFloat(0.0f);
    public bool IsReady { get; private set; } = false;
    
    public virtual void Initialize()
    {
        if (Elympics.IsClient)
        {
            appliedModifiers.ValueChanged += ApplyModifiers;
        }

        if (!Elympics.IsServer) return;
        CurrentTimeOfMatchRemaining.Value = matchLength;
        CurrentTimeToStartMatch.Value = preGameTime;
        PostGameTime.Value = postGameTime;
        GetComponent<StartGameController>().drawTarotCards();
        IsReady = true;
        
        
    }
    
    public virtual void ElympicsUpdate()
    {
        CurrentTimeToStartMatch.Value -= Elympics.TickDuration;
        if (!(CurrentTimeToStartMatch.Value < 0.0f)) return;
        if (!appliedModifiers.Value)
        {
            ApplyModifiers(false,true);
            appliedModifiers.Value = true;
        }
        CurrentTimeOfMatchRemaining.Value -= Elympics.TickDuration;
        matchTime.Value = CurrentTimeOfMatchRemaining.Value > 0.0f;
        if (matchTime.Value) return;
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

    private bool GetVotingResults()
    {
        var players = playerProvider.AllPlayersInScene;
        int votes = 0;
        foreach (var player in players)
        {
            if (player.GetComponent<PlayerVote>().vote.Value)
            {
                votes++;
            }
        }
        Debug.Log($"VOTES: {votes}");
        return votes >= 2;
    }

    private void ApplyModifiers(bool oldVal, bool newVal)
    {
        if (!newVal) return;
        if (!GetVotingResults())
        {
            GetComponent<StartGameController>().ApplyModifiers();
        }
    }
}
