using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject button;

    private void Start()
    {
        button.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleButton();
        }
    }

    private void ToggleButton()
    {
        button.SetActive(!button.activeSelf); 
    }

}

