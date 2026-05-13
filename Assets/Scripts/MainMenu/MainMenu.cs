using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "Level1SceneFinal";
    [SerializeField] private GameObject creditsPanel;
    private bool isOpen = false;

    // Start Game
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Open Credits
    public void OpenCredits()
    {
        //creditsPanel.SetActive(true);
        isOpen = !isOpen;

        creditsPanel.SetActive(isOpen);
    }
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    // Quit Game
    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}