using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using StarterAssets;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    
     
    [SerializeField] private Slider mouseSensitivitySlider;
    [Header("UI")]
    [SerializeField] private GameObject SettingsMenuPanel;
    [SerializeField] private GameObject PauseGamePanel;
    [Header("Variables")]
    private bool isOpened = false;
    public bool invertMouseX = false;
    public bool invertMouseY = false;
    public float mouseSensitivity = 10.0f;
    public float brightness = 1.0f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadSettings();
    }
    void Start()
    {
       
        SettingsMenuPanel.SetActive(false);//closed when start
    }
    // void Update()
    // {
        
    // }

    public void OpenSettings()
    {
        
        SettingsMenuPanel.SetActive(true);
        PauseGamePanel.SetActive(false);
        
    }
    public void CloseSettings()
    {
        SettingsMenuPanel.SetActive(false);
        PauseGamePanel.SetActive(true);
    }
    public void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
    }

    public void SetInvertMouseX(bool value)
    {
        invertMouseX = value;
        PlayerPrefs.SetInt("InvertMouseX", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetInvertMouseY(bool value)
    {
        invertMouseY = value;
        PlayerPrefs.SetInt("InvertMouseY", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetBrightness(float value)
    {
        brightness = value;
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
    }
    private void LoadSettings()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);//1.0 = default value if the player has never changed the setting.
        invertMouseX = PlayerPrefs.GetInt("InvertMouseX", 0) == 1;
        invertMouseY = PlayerPrefs.GetInt("InvertMouseY", 0) == 1;
        brightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
    }
}
