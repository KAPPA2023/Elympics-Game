using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Elympics;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private MatchmakingManager matchmakingManager;
    public void PlayGame()
    {
        matchmakingManager.PlayOnline();
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
