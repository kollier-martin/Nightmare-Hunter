using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("MasterVolume", 0.0f);
    }

    public void SetLevel()
    {
        float sliderValue = slider.value;
        mixer.SetFloat("Music", sliderValue);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetFXLevel()
    {
        float sliderValue = slider.value;
        mixer.SetFloat("FX", sliderValue);
        PlayerPrefs.SetFloat("FXVolume", sliderValue);
    }

    public void SetMasterLevel()
    {
        float sliderValue = slider.value;
        mixer.SetFloat("Master", sliderValue);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void Reset()
    {
        mixer.SetFloat("Master", 0.0f);
        mixer.SetFloat("Music", 0.0f);
        mixer.SetFloat("FX", 0.0f);
    }
}
