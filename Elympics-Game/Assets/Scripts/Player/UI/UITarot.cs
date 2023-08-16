using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UITarot : MonoBehaviour
{
    [SerializeField] private Image[] slots;
    [SerializeField] private TextMeshProUGUI[] texts;
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
                texts[0].text = "Cats ran out of lives \n 1KOT1KILL";
                break;
            case 1:
                sprite = GetSprite("The_Moon");
                texts[0].text = "Jump higher";
                break;
            case 2:
                sprite = GetSprite("The_Justice");
                texts[0].text = "Spells are balanced";
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
                texts[1].text = "More platforms to jump on :3";
                break;
            case 1:
                sprite = GetSprite("The_Fool");
                texts[1].text = "Better don't fall :3";
                break;
            case 2:
                sprite = GetSprite("The_Chariot");
                texts[1].text = "Wild MOVING PLATFORM appeared! (Broken rn)";
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
                texts[2].text = "Gambling (TBA)";
                break;
            case 1:
                sprite = GetSprite("The_Ace_of_Wands");
                texts[2].text = "More spells pickups";
                break;
            case 2:
                sprite = GetSprite("The_Magician");
                texts[2].text = "(TBA)";
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
