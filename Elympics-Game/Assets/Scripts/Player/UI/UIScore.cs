using TMPro;
using System;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private PlayerProvider playerProvider;
    private PlayerData[] players;
    
    private void Start()
    {
        if (playerProvider.IsReady)
            SubscribeToStatsController();
        else
            playerProvider.IsReadyChanged += SubscribeToStatsController;
    }
    
    private void SubscribeToStatsController()
    {
        players = playerProvider.AllPlayersInScene;
        foreach (var player in players)
        {
            player.Score.ValueChanged += UpdateScore;
        }
    }

    private void UpdateScore(int oldVal, int newVal)
    {
        String score = "";
        foreach (var player in players)
        {
            score += $"{player.playerName}: {player.Score.Value} \n";
        }
        text.text = score;
    }
}
