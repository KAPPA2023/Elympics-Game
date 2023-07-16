using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : ElympicsMonoBehaviour, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] private float timeToStartMatch = 1.0f;

    [SerializeField] private GameObject arena;
    
    public ElympicsFloat CurrentTimeToStartMatch { get; } = new ElympicsFloat(0.0f);
	
    private ElympicsBool gameInitializationEnabled = new ElympicsBool(false);
    
    public void InitializeMatch()
    {
        CurrentTimeToStartMatch.Value = timeToStartMatch;
        gameInitializationEnabled.Value = true;

        if (Elympics.IsServer)
        {
            int arenaModifier = Random.Range(1, 3);
            Color color = new Color();
            switch (arenaModifier)
            {
                case 1: color = Color.blue; break;
                case 2: color = Color.green; break;
                case 3: color = Color.red; break;
            }
            arena.GetComponent<Renderer>().material.SetColor("_Color",color);
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
    }
}
