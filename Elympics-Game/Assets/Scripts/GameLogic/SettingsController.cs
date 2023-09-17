using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    private string height = "Screenmanager Resolution Height";
    [SerializeField] private GameObject button;
    private int qualityLevel;
    void Start()
    {
        if (!PlayerPrefs.HasKey("QualityLevel"))
        { 
            PlayerPrefs.SetInt("QualityLevel", 1);
        }
        LoadSetting();
        button.GetComponentInChildren<TextMeshProUGUI>().text = QualitySettings.names[qualityLevel];
    }
    // Start is called before the first frame update
    public void ApplySettings()
    {
        qualityLevel++;
        if (qualityLevel >= 4) qualityLevel = 0;
        QualitySettings.SetQualityLevel(qualityLevel);
        PlayerPrefs.SetInt("QualityLevel", qualityLevel);
        button.GetComponentInChildren<TextMeshProUGUI>().text = QualitySettings.names[qualityLevel];
    }

    private void LoadSetting()
    { 
        qualityLevel = PlayerPrefs.GetInt("QualityLevel");
        QualitySettings.SetQualityLevel(qualityLevel);
    }
}






