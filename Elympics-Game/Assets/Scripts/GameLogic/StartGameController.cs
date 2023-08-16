using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameController : ElympicsMonoBehaviour
{
    [SerializeField] private GameObject basicAreaRoot;
    [SerializeField] private GameObject additionalPlatformsRoot;
    [SerializeField] private GameObject movingPlatformsRoot;
    [SerializeField] private GameObject areaWithHolesRoot;
    [SerializeField] private GameObject additionalSpellPickupsRoot;
    public ElympicsInt staticGameModifier = new ElympicsInt(-1);
    public ElympicsInt dynamicGameModifier = new ElympicsInt(-1);
    public ElympicsInt playerModifier = new ElympicsInt(-1);
    public ElympicsBool IsReady = new ElympicsBool(false);
    [SerializeField] private PlayerProvider playerProvider;


    public void drawTarotCards()
    {
        staticGameModifier.Value = UnityEngine.Random.Range(0, 3);
        dynamicGameModifier.Value = UnityEngine.Random.Range(0, 3);
        playerModifier.Value = UnityEngine.Random.Range(0, 3);
        IsReady.Value = true;
    }

    public void ApplyModifiers()
    {
        if (playerProvider.IsReady)
        {
            ApplyPlayerModifiers();
        }
        else
        {
            playerProvider.IsReadyChanged += ApplyPlayerModifiers;
        }

        ApplyArenaModifiers();
    }

    private void ApplyPlayerModifiers()
    {
        var players = playerProvider.AllPlayersInScene;

        foreach (var player in players)
        {
            player.ApplyModifier(playerModifier.Value);
        }
    }

    private void ApplyArenaModifiers()
    {
        switch (staticGameModifier.Value)
        {
            case 0:
                additionalPlatformsRoot.SetActive(true); 
                break;
            case 1:
                basicAreaRoot.SetActive(false);
                areaWithHolesRoot.SetActive(true);
                break;
            case 2:
                //TODO: fix it xdd
                //movingPlatformsRoot.SetActive(true);
                break;
        }
    }

}
