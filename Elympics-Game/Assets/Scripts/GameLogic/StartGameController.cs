using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameController : ElympicsMonoBehaviour
{
    private ElympicsInt staticGameModifier = new ElympicsInt(-1);
    private ElympicsInt dynamicGameModifier = new ElympicsInt(-1);
    private ElympicsInt playerModifier = new ElympicsInt(-1);
    public bool IsReady { get; private set; } = false;
    public event Action IsReadyChanged = null;
    [SerializeField] private PlayerProvider playerProvider;


    public void drawTarotCards()
    {
        staticGameModifier.Value = UnityEngine.Random.Range(0, 3);
        dynamicGameModifier.Value = UnityEngine.Random.Range(0, 3);
        playerModifier.Value = UnityEngine.Random.Range(0, 3);

        IsReady = true;
        IsReadyChanged?.Invoke();
        Debug.Log("player " + playerModifier.Value);
        Debug.Log("dynamic " + dynamicGameModifier.Value);
        Debug.Log("static " + staticGameModifier.Value);
    }

    public void applyModifiers()
    {
        if (playerProvider.IsReady)
        {
            applyPlayerModifiers();
        }
        else
        {
            playerProvider.IsReadyChanged += applyPlayerModifiers;
        }
    }

    private void applyPlayerModifiers()
    {
        var players = playerProvider.AllPlayersInScene;

        foreach (var player in players)
        {
            player.ApplyModifier(playerModifier.Value);
        }
    }

}
