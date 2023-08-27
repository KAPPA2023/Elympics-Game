using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Elympics;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private MatchmakingManager matchmakingManager;

    public void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void PlayGame()
    {
        matchmakingManager.PlayOnline();
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
