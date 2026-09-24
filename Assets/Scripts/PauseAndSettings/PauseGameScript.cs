using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem;
using StarterAssets;
using UnityEngine.SceneManagement; 

public class PauseGameScript : MonoBehaviour
{

    [Header("Input")]
    public ThirdPersonController thirdPersonController;
    [Header("UI")]
    [SerializeField] private GameObject PauseMenuPanel;
    [Header("Variables")]
    private bool isPaused = false;
    [SerializeField] private string mainMenuScene = "MainMenu";

    void Start()
    {
        thirdPersonController = ThirdPersonController.Instance;
        thirdPersonController.OnPauseGame += PauseGame;//
        PauseMenuPanel.SetActive(false);//closed when start
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

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuScene);
    }
}