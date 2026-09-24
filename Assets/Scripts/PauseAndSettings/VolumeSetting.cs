using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private Slider ambientVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private AudioMixer ambientMixer;
    [SerializeField] private AudioMixer SFXMixer;

    private void Start()
    {
        SetAmbientVolume(PlayerPrefs.GetFloat("SavedAmbientVolume", 100f));   
        SetSFXVolume(PlayerPrefs.GetFloat("SavedSFXVolume", 100f));   
    }
    public void SetAmbientVolume(float _value)
    {
        if(_value < 1)
        {
            _value = 0.001f; // Prevents log10(0) error
        }

        RefreshAmbientSlider(_value);
        PlayerPrefs.SetFloat("SavedAmbientVolume", _value);
        PlayerPrefs.SetFloat("SavedSFXVolume", _value);
        ambientMixer.SetFloat("AmbientVolume", Mathf.Log10(_value/100) * 20); // Convert linear volume to decibels
    }
    public void SetSFXVolume(float _value)
    {
        if(_value < 1)
        {
            _value = 0.001f; // Prevents log10(0) error
        }

        RefreshSFXSlider(_value);
        PlayerPrefs.SetFloat("SavedAmbientVolume", _value);
        PlayerPrefs.SetFloat("SavedSFXVolume", _value);
        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(_value/100) * 20); // Convert linear volume to decibels
    }
    public void SetAmbientVolumeFromSlider()
    {
        SetAmbientVolume(ambientVolumeSlider.value);
    }
    public void SetSFXVolumeFromSlider()
    {
        SetSFXVolume(SFXVolumeSlider.value);
    }
    public void RefreshAmbientSlider(float _value)
    {
        ambientVolumeSlider.value = _value;
    }
    public void RefreshSFXSlider(float _value)
    {
        SFXVolumeSlider.value = _value;
    }
}
