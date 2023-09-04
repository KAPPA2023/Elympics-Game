using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private AudioSource testAudio = null;
    private bool initialChange = true;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("VolumeValue"))
        { 
            PlayerPrefs.SetFloat("VolumeValue", 0.5f);
        }
        LoadVolume();
    }

    public void SaveVolumeButton()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        LoadVolume();
    }
    
    private void LoadVolume()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }

    public void OnValueChanged()
    {
        if (!initialChange)
        {
            SaveVolumeButton();
            if (!testAudio.isPlaying)
            {
                testAudio.Play();
            }
        }
        else
        {
            initialChange = false;
        }
    }
}
