using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StartGameController : ElympicsMonoBehaviour
{
    [SerializeField] private GameObject basicAreaRoot;
    [SerializeField] private GameObject additionalPlatformsRoot;
    [SerializeField] private GameObject additionalJumpPadsRoot;
    [SerializeField] private GameObject areaWithHolesRoot;
    [SerializeField] private GameObject additionalSpellPickupsRoot;
    public ElympicsInt staticGameModifier = new ElympicsInt(-1);
    public ElympicsInt playerModifier = new ElympicsInt(-1);
    public ElympicsInt enchancedSpell = new ElympicsInt(-1);
    public ElympicsBool IsReady = new ElympicsBool(false);
    [SerializeField] private PlayerProvider playerProvider;


    public void drawTarotCards()
    {
        enchancedSpell.Value = UnityEngine.Random.Range(0, 6);
        playerModifier.Value = UnityEngine.Random.Range(0, 3);
        staticGameModifier.Value = UnityEngine.Random.Range(0, 4);
        IsReady.Value = true;
    }

    public void ApplyFirstModifier()
    {
        playerProvider.UpdatePlayerProvider();
        
        var players = playerProvider.AllPlayersInScene;
        foreach (var player in players)
        {
            player.ApplySpellEnhance(enchancedSpell.Value);
        }
    }

    public void ApplySecondModifier()
    {
        playerProvider.UpdatePlayerProvider();
        
        var players = playerProvider.AllPlayersInScene;
        foreach (var player in players)
        {
            player.ApplyModifier(playerModifier.Value);
        }
    }
    public void ApplyThirdModifier()
    {
        switch (staticGameModifier.Value)
        {
            case 0:
                //mystery spell pickups
                break;
            case 1:
                //more spell pickups
                additionalSpellPickupsRoot.SetActive(true);
                break;
            case 2:
                //More platforms
                additionalPlatformsRoot.SetActive(true); 
                break;
            case 3:
                //map with holes
                basicAreaRoot.SetActive(false);
                areaWithHolesRoot.SetActive(true);
                break;
            case 4:
                //more jump pads
                additionalJumpPadsRoot.SetActive(true);
                break;
        }
    }
}
