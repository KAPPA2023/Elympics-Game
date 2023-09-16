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
    [SerializeField] private GameObject[] voteMarks;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private TextMeshProUGUI[] voteCounters;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private StartGameController startGameController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerProvider playerProvider;
    [SerializeField] private GameObject selectionImage;

    private Action _votesChanged;
    private int[] _voteCounters;
    private void Start()
    {
        _voteCounters = new int[3];
        _votesChanged += UpdateVotes;
        if (startGameController.IsReady.Value)
        {
            DisplayCards(false, true);
        }
        else
        {
            startGameController.IsReady.ValueChanged += DisplayCards;
        }

        if (playerProvider.IsReady)
        {
            SubscribeToPlayerVote();
        }
        else
        {
            playerProvider.IsReadyChanged += SubscribeToPlayerVote;
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

    private void SubscribeToPlayerVote()
    {
        playerProvider.ClientPlayer.GetComponent<PlayerVote>().selectedCard.ValueChanged += UpdateSelectedCard;
        
        var votes = playerProvider.ClientPlayer.GetComponent<PlayerVote>().votes;
        votes.Values[0].ValueChanged += UpdateVoteDisplay1;
        votes.Values[1].ValueChanged += UpdateVoteDisplay2;
        votes.Values[2].ValueChanged += UpdateVoteDisplay3;

        var players = playerProvider.AllPlayersInScene;
        foreach (var player in players)
        {
            var playerVote = player.GetComponent<PlayerVote>().votes;
            playerVote.Values[0].ValueChanged += UpdateVote1;
            playerVote.Values[1].ValueChanged += UpdateVote2;
            playerVote.Values[2].ValueChanged += UpdateVote3;
        }
    }

    private void UpdateVotes()
    {
        for (var i = 0; i < 3; i++)
        {
            voteCounters[i].text = $"{_voteCounters[i]}/2";
        };
    }

    private void UpdateSelectedCard(int oldVal, int newVal)
    {
        var position = selectionImage.transform.position;
        selectionImage.transform.position = new Vector3(slots[newVal].transform.position.x,position.y,position.z); 
    }

    private void UpdateVoteDisplay1(bool oldVal, bool newVal)
    {
        voteMarks[0].SetActive(newVal);
    }
    private void UpdateVoteDisplay2(bool oldVal, bool newVal)
    {
        voteMarks[1].SetActive(newVal);
    }
    private void UpdateVoteDisplay3(bool oldVal, bool newVal)
    {
        voteMarks[2].SetActive(newVal);
    }
    private void UpdateVote1(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            _voteCounters[0] += 1;
        }
        else
        {
            _voteCounters[0] -= 1;
        }
        _votesChanged?.Invoke();
    }
    private void UpdateVote2(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            _voteCounters[1] += 1;
        }
        else
        {
            _voteCounters[1] -= 1;
        }
        _votesChanged?.Invoke();
    }
    private void UpdateVote3(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            _voteCounters[2] += 1;
        }
        else
        {
            _voteCounters[2] -= 1;
        }
        _votesChanged?.Invoke();
    }
}
