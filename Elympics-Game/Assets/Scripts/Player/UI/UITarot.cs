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
        SetupFirstCard(startGameController.enchancedSpell.Value);
        SetupSecondCard(startGameController.playerModifier.Value);
        SetupThirdCard(startGameController.staticGameModifier.Value);
    }

    private void SetupFirstCard(int spell)
    {
        Sprite sprite = GetSprite("The_Justice");
        texts[0].text = $"{(Spells)spell} is enhanced";
        slots[0].sprite = sprite;
    }
    private void SetupSecondCard(int modifier)
    {
        Sprite sprite = sprites[0];
        switch (modifier)
        {
            case 0:
                sprite = GetSprite("The_Death");
                texts[1].text = "Cats ran out of lives \n 1KOT1KILL";
                break;
            case 1:
                sprite = GetSprite("The_Moon");
                texts[1].text = "Jump higher";
                break;
            case 2:
                sprite = GetSprite("The_Magician");
                texts[1].text = "Cast whatever you want";
                break;
        }
        slots[1].sprite = sprite;
    }
    private void SetupThirdCard(int modifier)
    {
        Sprite sprite = sprites[0];
        switch (modifier)
        {
            case 0:
                sprite = GetSprite("The_Wheel_of_Fortune");
                texts[2].text = "Mystery spell pickups";
                break;
            case 1:
                sprite = GetSprite("The_Ace_of_Wands");
                texts[2].text = "More spells pickups";
                break;
            case 2:
                sprite = GetSprite("The_Tower");
                texts[2].text = "More platforms to jump on :3";
                break;
            case 3:
                sprite = GetSprite("The_Fool");
                texts[2].text = "Better don't fall :3";
                break;
            case 4:
                sprite = GetSprite("The_Chariot");
                texts[2].text = "More JUMP PADS";
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
