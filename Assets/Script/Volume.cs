using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider =null;

    public void VolumeSlider(float volume)
    {

    }
    public void SaveVolume()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeValue);
        LoadValues();
    }
    void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("Volume");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadValues();
    }

    
}
