using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;
using TMPro;

public class MatchmakingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private TextMeshProUGUI matchStatus;
    
    private void Start()
    {
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted += DisplayMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingMatchFound += DisplayMatchFound;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed += DisplayMatchmakingError;
    }

    private void OnDestroy()
    {
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted -= DisplayMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingMatchFound -= DisplayMatchFound;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed -= DisplayMatchmakingError;
    }
    
    public void PlayOnline()
    {
        ElympicsConfig.Load().SwitchGame(0);
        ElympicsLobbyClient.Instance.PlayOnlineInRegion(null, null,null, "Default");
    }

    public void Tutorial()
    {
        ElympicsConfig.Load().SwitchGame(1);
        ElympicsLobbyClient.Instance.PlayOffline();
    }
    
    private void DisplayMatchmakingStarted()
    {
        matchStatus.text = "looking for game...";

        foreach (var button in buttons)
        {
            button.SetActive(false);
        }
    }

    private void DisplayMatchFound(Guid guid)
    {
        matchStatus.text  = "game found";
    }
    
    private void DisplayMatchmakingError((String error,Guid guid)info)
    {
        matchStatus.text = "unlucky ... maybe try again?";
        
        foreach (var button in buttons)
        {
            button.SetActive(true);
        }
    }
}
