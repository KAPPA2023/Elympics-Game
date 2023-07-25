using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Elympics;
using Unity.VisualScripting;

public class EndgameController : MonoBehaviour
{
    [SerializeField] private PlayerProvider playerProvider = null;
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private GameObject screen;

    private void Start()
    {
        SubscribeToGameManager();
    }

    private void SubscribeToGameManager()
    {
        gameManager.gameEnded.ValueChanged += DisplayEndgameInfo;
    }

    private void DisplayEndgameInfo(bool oldState,bool newState)
    {
        if (newState)
        {
            screen.SetActive(true);
            PlayerData[] players = playerProvider.AllPlayersInScene;
            TextMeshProUGUI text = screen.GetComponentInChildren<TextMeshProUGUI>();
        
            if (players[0].Score.Value == players[1].Score.Value)
            {
                text.SetText($"DRAW \n {players[0].Score.Value}:{players[1].Score.Value}");
            }
            else
            {
                int winnerID = (players[0].Score.Value > players[1].Score.Value) ? 0 : 1;
                text.SetText($"Player {players[winnerID].playerName} won \n {players[winnerID].Score.Value}:{players[1 - winnerID].Score.Value}");
            }
        }
    }
}
