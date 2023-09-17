using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Elympics;
using UnityEngine.SceneManagement;

public class EndgameController : MonoBehaviour
{
    [SerializeField] private PlayerProvider playerProvider = null;
    [SerializeField] private GameManager gameManager = null;
    private bool gameEnded = false;
    
    [SerializeField] private GameObject screen;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private GameObject pointer;
    
    

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

            var leaderboard = GetLeaderboard();
            for (var i = 0; i < leaderboard.Count; i++)
            {
                var texts = slots[i].GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = $"{leaderboard[i].Name}";
                texts[1].text = $"{leaderboard[i].Score}";
                texts[2].text = $"{leaderboard[i].Place}";
                if (leaderboard[i].ID != playerProvider.ClientPlayer.PlayerId) continue;
                var position = pointer.transform.position;
                pointer.transform.position = new Vector3(position.x, slots[i].transform.position.y, position.z);
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

    private List<PlayerScore> GetLeaderboard()
    {
        PlayerData[] players = playerProvider.AllPlayersInScene;
        var sorted = players.OrderByDescending(player => player.Score.Value);
        List<PlayerScore> scores = new List<PlayerScore>();

        foreach (var player in sorted)
        {
            scores.Add(new PlayerScore(player));
        }

        for (var i = 0; i < scores.Count; i++)
        {
            if (i > 0)
            {
                if (scores[i].Score < scores[i-1].Score)
                {
                    scores[i].Place = scores[i - 1].Place + 1;
                }
                else
                {
                    scores[i].Place = scores[i - 1].Place;
                }
            }
            else
            {
                scores[i].Place = 1;
            }
        }
        
        return scores;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private class PlayerScore
    {
        public string Name;
        public int ID;
        public int Place;
        public int Score;

        public PlayerScore(PlayerData playerData)
        {
            Name = playerData.playerName;
            ID = playerData.PlayerId;
            Score = playerData.Score;
        }
    }
    
}
