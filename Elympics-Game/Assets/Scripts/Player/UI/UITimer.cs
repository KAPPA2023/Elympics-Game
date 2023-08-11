using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private TextMeshProUGUI _timer;

    public void Awake()
    {
        _timer = GetComponent<TextMeshProUGUI>();

        gameManager.CurrentTimeOfMatchRemaining.ValueChanged += UpdateTimer;
    }
    
    private void UpdateTimer(float oldVal, float newVal)
    {
        if (newVal <= 0.0f)
        {
            _timer.text = "";
            return;
        }
        _timer.text = $"{(int)newVal}";
    }
    
}
