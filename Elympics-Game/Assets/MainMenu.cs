using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Elympics;

public class MainMenu : MonoBehaviour
{
    private bool searchInProgress = false;
    
    public void PlayGame()
    {
        searchInProgress = true;
        if (searchInProgress) return;
        ElympicsLobbyClient.Instance.PlayOnlineInRegion(null, null,null, "Default");
    }

    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
