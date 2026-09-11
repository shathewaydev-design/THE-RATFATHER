using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer ambientMixer;
    [SerializeField] private AudioMixer SFXMixer;

    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedAmbientVolume", 100f));   
        SetVolume(PlayerPrefs.GetFloat("SavedSFXVolume", 100f));   
    }
    public void SetVolume(float _value)
    {
        if(_value < 1)
        {
            _value = 0.001f; // Prevents log10(0) error
        }

        RefreshSlider(_value);
        PlayerPrefs.SetFloat("SavedAmbientVolume", _value);
        PlayerPrefs.SetFloat("SavedSFXVolume", _value);
        ambientMixer.SetFloat("AmbientVolume", Mathf.Log10(_value/100) * 20); // Convert linear volume to decibels
        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(_value/100) * 20); // Convert linear volume to decibels
    }
    public void SetVolumeFromSlider()
    {
        SetVolume(volumeSlider.value);
    }
    public void RefreshSlider(float _value)
    {
        volumeSlider.value = _value;
    }
}
