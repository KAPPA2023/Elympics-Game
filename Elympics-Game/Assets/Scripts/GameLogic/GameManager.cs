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

        if (playerProvider.IsReady)
        {
            playerProvider.UpdatePlayerProvider();
        }
        else
        {
            playerProvider.IsReadyChanged += playerProvider.UpdatePlayerProvider;
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

    private bool[] GetVotingResults()
    {
        var players = playerProvider.AllPlayersInScene;
        var votes = new int[3];
;        foreach (var player in players)
        {
            for (var i = 0; i < 3; i++)
            {
                if (player.GetComponent<PlayerVote>().votes.Values[i].Value)
                {
                    votes[i] += 1;
                }
            }
        }

        var results = new bool[3];
        for (var i = 0; i < 3; i++)
        {
            if (votes[i] >= 2)
            {
                results[i] = true;
            }
        }
        
        return results;
    }

    private void ApplyModifiers(bool oldVal, bool newVal)
    {
        if (!newVal) return;
        var startGameController = GetComponent<StartGameController>();
        var results = GetVotingResults();
        if (!results[0])
        {
            startGameController.ApplyFirstModifier();
        }
        if (!results[1])
        {
            startGameController.ApplySecondModifier();
        }
        if (!results[2])
        {
            startGameController.ApplyThirdModifier();
        }
    }
}
