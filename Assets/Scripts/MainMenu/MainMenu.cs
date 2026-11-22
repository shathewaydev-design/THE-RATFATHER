using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "Level1SceneFinal";
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject newGamePanel;
    //Location: Canvas> New/Continue
    [SerializeField] private Button continueButton;
    //Location: Canvas> New/Continue > CONTINUE
    private bool isOpen = false;

    private void Start()
    {
        if(DataPersistenceManager.Instance.HasGameData())
        {
            //if we have game data, enable the continue button
            continueButton.interactable = true;
        }
        else
        {
            //if we dont have game data, disable the continue button
            continueButton.interactable = false;
        }
    }
    
    // Start Game
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void OnNewGameClicked()
    {
        //create a new game - which will initialize the game data 
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync(gameSceneName);
        //load the gameplay scene - which will in turn save the game 
        //because of OnSceneUnloaded() in DataPersistenceManager
    }
    public void OnContinueGameClicked()
    {
        //DataPersistenceManager.Instance.LoadGame();
        //load the next scene - which will in turn save the game 
        // because of OnSceneUnloaded() in DataPersistenceManager
        SceneManager.LoadSceneAsync(gameSceneName);
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
    public void OpenNewGamePanel()
    {
        newGamePanel.SetActive(true);
    }

    public void CloseNewGamePanel()
    {
        newGamePanel.SetActive(false);
    }
    // Quit Game
    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}