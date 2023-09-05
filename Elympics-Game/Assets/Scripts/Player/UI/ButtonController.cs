using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button yourButton; 

    private void Start()
    {
        yourButton.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        SceneManager.LoadScene(0);
    }
}
