using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerProvider playersProvider = null;
    [SerializeField] private Slider healthSlider = null;

    private void Start()
    {
        if (playersProvider.IsReady)
            SubscribeToStatsController();
        else
            playersProvider.IsReadyChanged += SubscribeToStatsController;
    }

    private void SubscribeToStatsController()
    {
        var clientPlayerData = playersProvider.ClientPlayer;
        clientPlayerData.GetComponent<StatsController>().HealthValueChanged += UpdateHealthView;
    }

    private void UpdateHealthView(float currentHealth, float maxHealth)
    {
        healthSlider.value = currentHealth / maxHealth * healthSlider.maxValue;
    }
}
