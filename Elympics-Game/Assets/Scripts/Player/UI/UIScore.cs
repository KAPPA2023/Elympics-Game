using TMPro;
using System;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private PlayerProvider playerProvider;
    [SerializeField] private GameManager gameManager;
    private PlayerData[] players;
    
    private void Start()
    {
        gameManager.matchTime.ValueChanged += SubscribeToStatsController;
    }
    
    private void SubscribeToStatsController(bool oldVal, bool newVal)
    {
        players = playerProvider.AllPlayersInScene;
        foreach (var player in players)
        {
            player.Score.ValueChanged += UpdateScore;
        }
        UpdateScore(0,0);
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
