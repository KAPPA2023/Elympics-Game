using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using Elympics.Models.Matchmaking;
using TMPro;

public class MatchmakingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private TextMeshProUGUI matchStatus;
    
    private void Start()
    {
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted += DisplayMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingMatchFound += _ => DisplayMatchFound();
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed += _ => DisplayMatchmakingError();
    }

    private void OnDestroy()
    {
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted -= DisplayMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingMatchFound -= _ => DisplayMatchFound();
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed -= _ => DisplayMatchmakingError();
    }
    
    public void PlayOnline()
    {
        ElympicsLobbyClient.Instance.PlayOnlineInRegion(null, null,null, "Default");
        
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted += DisplayMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingMatchFound += _ => DisplayMatchFound();
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed += _ => DisplayMatchmakingError();
        
    }
    
    private void DisplayMatchmakingStarted()
    {
        matchStatus.text = "looking for game :)";

        foreach (var button in buttons)
        {
            button.SetActive(false);
        }
    }

    private void DisplayMatchFound()
    {
        matchStatus.text  = "game found POG";
    }
    
    private void DisplayMatchmakingError()
    {
        matchStatus.text = "unlucky ... maybe try again?";
        
        foreach (var button in buttons)
        {
            button.SetActive(true);
        }
    }
}
