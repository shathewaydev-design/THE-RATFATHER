using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using StarterAssets;

public class SettingsScript : MonoBehaviour
{
    [Header("Input")]
    public ThirdPersonController thirdPersonController;
    [Header("UI")]
    [SerializeField] private GameObject PauseMenuPanel;
    [Header("bool")]
    private bool isPaused = false;

    void Start()
    {
        thirdPersonController = ThirdPersonController.Instance;
        thirdPersonController.OnPauseGame += PauseGame;
        PauseMenuPanel.SetActive(false);//closed when start
    }
    void Update()
    {
        
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0.0f;

            PauseMenuPanel.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1.0f;

            PauseMenuPanel.SetActive(false);
        }
    }
}
