using Elympics;
using UnityEngine;

public class GameManager : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private float matchLength = 120.0f;
    [SerializeField] private float postGameTime = 5.0f;
    [SerializeField] private float preGameTime = 5.0f;
    private ElympicsBool appliedModifiers = new ElympicsBool(false);
    private bool _modifiersApplie = false;

    public ElympicsBool matchTime = new ElympicsBool(false);
    public ElympicsBool gameEnded = new ElympicsBool(false);
    public ElympicsFloat CurrentTimeOfMatchRemaining { get; } = new ElympicsFloat(0.0f);
    public ElympicsFloat CurrentTimeToStartMatch { get; } = new ElympicsFloat(0.0f);
    public ElympicsFloat PostGameTime { get; } = new ElympicsFloat(0.0f);
    public bool IsReady { get; private set; } = false;
    
    public virtual void Initialize()
    {
        if (appliedModifiers.Value && !_modifiersApplie)
        {
            applyModifiers(false,true);
        }
        else
        {
            appliedModifiers.ValueChanged += applyModifiers;
        }
        
        if (!Elympics.IsServer) return;
        CurrentTimeOfMatchRemaining.Value = matchLength;
        CurrentTimeToStartMatch.Value = preGameTime;
        PostGameTime.Value = postGameTime;
        GetComponent<StartGameController>().drawTarotCards();
        appliedModifiers.Value = true;

        IsReady = true;
    }
    
    public virtual void ElympicsUpdate()
    {
        CurrentTimeToStartMatch.Value -= Elympics.TickDuration;
        if (!(CurrentTimeToStartMatch.Value < 0.0f)) return;
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

    private void applyModifiers(bool oldVal, bool newVal)
    {
        if (!newVal) return;
        _modifiersApplie = true;
        GetComponent<StartGameController>().ApplyModifiers();
    }
}
