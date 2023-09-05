using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject button;
    [SerializeField] private GameObject background;
    [SerializeField] private InputProvider playerInput;

    private void Start()
    {
        button.SetActive(false);
        background.SetActive(false);
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
        background.SetActive(button.activeSelf);
        playerInput.enabled = !button.activeSelf;

    }

}

