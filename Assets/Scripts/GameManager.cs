using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;
using StarterAssets;


public class GameManager : MonoBehaviour
{
    //public int currCurrency;

    [SerializeField] private List<ComicCutscene> InGameScenes;

    public GameObject JunkYardEntrance;

    public static GameManager Instance;
    public PlayerState playerState;

    public bool correctCheese = true; // determined by npc preference (desc = clue). for now always assume true
    public ConversationData sellConvo;

    public bool firstBossDefeated;

    public int currHealth = 0;
    public Texture[] health;
    public RawImage healthBar;


    //public VideoPlayer videoPlayer;
    //public RawImage videoRenderer;


    public int cutsceneIndex = 0;
    public GameObject blackScreenPanel;
    public TextMeshProUGUI cutsceneText;
    private bool cutsceneActive = false;
    public string[] bossCutsceneLines;

    public ThirdPersonController thirdPersonController; // movement reference

    public TutorialManager tutorialManager;

    bool introDone = false;
    public bool inBossScene = false;

    bool bossCutsceneStarted = false;

    private bool waitingForClick = false;



    private void Awake()
    {

        // Singleton pattern (simple version); avoid duplicates
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        playerState = new PlayerState
        {
            currency = 0,
            reputation = 0,
            currLevel = 1,
            playerHealth = 100,
            recruitedRats = new List<NPCProfile>(),
            soldTo = new List<NPCProfile>()

        };
        thirdPersonController = ThirdPersonController.Instance;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        //if (!introDone && !inBossScene)
        //{
        //    StartCoroutine(IntroSequence());
        //}

        //CutsceneManager.Instance.currCutscene = InGameScenes[0];
        if (!inBossScene) // ADDING MORE LEVELS -- CLEAN UP
        {
            CutsceneManager.Instance.currCutscene = InGameScenes[0];
            CutsceneManager.Instance.StartCutscene();
        }





    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log("List counts " + playerState.soldTo.Count + " " +  playerState.recruitedRats.Count );


        if (UIManager.Instance != null)
        {
            UIManager.Instance.ToggleLog();
            RecruitManager();
        }

        CheckStatus();

        if (!cutsceneActive)
            return;

        if (thirdPersonController.mouseClick.WasPressedThisFrame())
        {
            AdvanceCutscene();
        }



        //Debug.Log("Player health: " + playerState.playerHealth);
    }

    void AdvanceCutscene()
    {
        cutsceneIndex++;

        // if more lines exist
        if (cutsceneIndex < bossCutsceneLines.Length)
        {
            cutsceneText.text = bossCutsceneLines[cutsceneIndex];
        }
        else
        {
            EndCutscene();
        }
    }

    void EndCutscene()
    {
        cutsceneActive = false;
        blackScreenPanel.SetActive(false);
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        // continue game logic here
        //tutorialManager.currentStage = TutorialManager.TutorialStage.CompleteQuest;
    }


    //public IEnumerator IntroSequence()
    //{
    //    thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");

    //    // Make sure video object is active
    //    videoPlayer.gameObject.SetActive(true);

    //    videoPlayer.Prepare();
    //    yield return new WaitUntil(() => videoPlayer.isPrepared);

    //    videoPlayer.Play();

    //    // wait until it finishes properly
    //    yield return new WaitUntil(() => !videoPlayer.isPlaying);

    //    videoPlayer.gameObject.SetActive(false);
    //    videoRenderer.gameObject.SetActive(false);

    //    blackScreenPanel.SetActive(true);

    //    // yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
    //    yield return new WaitUntil(() => thirdPersonController.mouseClick.WasPressedThisFrame());

    //    blackScreenPanel.SetActive(false);

    //    thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");

    //    tutorialManager.currentStage = TutorialManager.TutorialStage.TalkToNPC;

    //    introDone = true;
    //}


    public void increaseCurrency(int increaseBy)
    {
        playerState.currency += increaseBy;
    }



    public void TakeDamage()
    {
        // for now all ways of taking damage hurt the player the same amount

        if (playerState.playerHealth <= 0)
        {
            DeathManager();
            return;
        }

        playerState.playerHealth -= 5;
        UpdatePlayerHealth();


    }

    private void DeathManager()
    {
        // game over -> fade to black?? May only be called for
        // boss level right now, so can be specific to that.

        // for boss level -> fade to black, start the boss level over

        Debug.Log("You Died!");
        // start whole scene over
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void RecruitRat(NPCProfile npc)
    {
        playerState.recruitedRats.Add(npc);
        UIManager.Instance.AddToRecruitLog(npc);

    }

    public void RecruitManager()
    {
        // loop through the recruited rats, 
        // check the type of things they bring back and at what rate,
        // add each ingrident they bring back to inventory.
    }

    //public void SellingManager()
    //{

    //}

    public void SellItem(FinalResultCheese cheese)
    {
        // logic will become more complicated later on
        // can check if rat wants the cheese here later

        // check if correct cheese
        // sell cheese here, increase reputation
        // remove specific cheese from inventory

        InventorySystem.Instance.RemoveFinalCheese(cheese, 1);
        //increaseCurrency(int.Parse(cheese.basePrice));
        DialogueManager.Instance.ResumeDialogue(true);
        UIManager.Instance.ToggleSellScreen();
    
        Debug.Log("Cheese sold!");
        //DialogueManager.Instance.StartConversation(sellConvo);

    }

    public bool CheckStatus()
    {
        if (bossCutsceneStarted)
            return true;

        if (playerState.soldTo.Count > 0 && playerState.recruitedRats.Count > 0)
        {
            thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
            FirstBossCutscene();
            bossCutsceneStarted = true;
            return true;


        }

        return false;
    }

    void FirstBossCutscene()
    {

        blackScreenPanel.SetActive(true);

        cutsceneIndex = 0;
        cutsceneActive = true;

        cutsceneText.text = bossCutsceneLines[cutsceneIndex];


    }

    public void UpdatePlayerHealth()
    {
        if (currHealth >= health.Length)
        {

            return;
        }

        healthBar.texture = health[currHealth];
        currHealth += 1;

    }


}
