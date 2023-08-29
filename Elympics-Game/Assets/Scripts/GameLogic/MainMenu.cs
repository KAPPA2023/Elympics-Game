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

    public void Play1V1()
    {
        matchmakingManager.PlayOnline("Default");
    }
    
    public void Play1V3()
    {
        matchmakingManager.PlayOnline("1v3");
    }

    public void PlayTutorial()
    { 
        matchmakingManager.Tutorial();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
