using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UITarot : MonoBehaviour
{
    [SerializeField] private Image[] slots;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private StartGameController startGameController;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        if (startGameController.IsReady.Value)
        {
            DisplayCards(false, true);
        }
        else
        {
            startGameController.IsReady.ValueChanged += DisplayCards;
        }

        gameManager.CurrentTimeToStartMatch.ValueChanged += OnMatchStart;
    }
    
    private void DisplayCards(bool oldVal, bool newVal)
    {
        SetupPlayerCard(startGameController.playerModifier.Value);
        SetupStaticCard(startGameController.staticGameModifier.Value);
        SetupDynamicCard(startGameController.dynamicGameModifier.Value);
    }

    private void SetupPlayerCard(int modifier)
    {
        Sprite sprite = sprites[0];
        switch (modifier)
        {
            case 0:
                sprite = GetSprite("The_Death"); 
                break;
            case 1:
                sprite = GetSprite("The_Moon"); 
                break;
            case 2:
                sprite = GetSprite("The_Justice"); 
                break;
        }
        slots[0].sprite = sprite;
    }
    private void SetupStaticCard(int modifier)
    {
        Sprite sprite = sprites[0];
        switch (modifier)
        {
            case 0:
                sprite = GetSprite("The_Tower"); 
                break;
            case 1:
                sprite = GetSprite("The_Fool"); 
                break;
            case 2:
                sprite = GetSprite("The_Chariot"); 
                break;
        }
        slots[1].sprite = sprite;
    }
    private void SetupDynamicCard(int modifier)
    {
        Sprite sprite = sprites[0];
        switch (modifier)
        {
            case 0:
                sprite = GetSprite("The_Wheel_of_Fortune"); 
                break;
            case 1:
                sprite = GetSprite("The_Ace_of_Wands"); 
                break;
            case 2:
                sprite = GetSprite("The_Magician"); 
                break;
        }
        slots[2].sprite = sprite;
    }

    private Sprite GetSprite(String name)
    {
        foreach(var sprite in sprites)
        {
            if(sprite.name == name)
            {
                return sprite;
            }
        }
        return sprites[0];
    }

    private void OnMatchStart(float oldVal, float newVal)
    {
        if (newVal >= 0) return;
        gameManager.CurrentTimeToStartMatch.ValueChanged -= OnMatchStart;
        this.GameObject().SetActive(false);
    }
}
