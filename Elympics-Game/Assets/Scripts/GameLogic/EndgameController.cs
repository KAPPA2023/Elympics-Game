using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Elympics;
using UnityEngine.SceneManagement;

public class EndgameController : MonoBehaviour
{
    [SerializeField] private PlayerProvider playerProvider = null;
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private GameObject screen;
    private bool gameEnded = false;

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
            gameEnded = true;
            screen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            TextMeshProUGUI text = screen.GetComponentInChildren<TextMeshProUGUI>();
            var winners = GetWinner();
            if (winners.Count == 1)
            {
                text.SetText($"Player {winners[0].playerName} won \n Points: {winners[0].Score.Value}");
            }
            else
            {
                String players = " ";
                foreach (var player in winners)
                {
                    players += player.playerName + " ";
                }
                text.SetText($"DRAW \n {players} \n SCORE: {winners[0].Score.Value}");
            }
        }
    }

    private void FixedUpdate()
    {
        if (!gameEnded) return;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private List<PlayerData> GetWinner()
    {
        PlayerData[] players = playerProvider.AllPlayersInScene;
        List<PlayerData> winners = new List<PlayerData>();
        int max_score = -100;
        foreach (var player in players)
        {
            if (player.Score.Value > max_score)
            {
                max_score = player.Score.Value;
            }
        }
        foreach (var player in players)
        {
            if (player.Score.Value == max_score)
            {
                winners.Add(player);
            }
        }
        return winners;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
