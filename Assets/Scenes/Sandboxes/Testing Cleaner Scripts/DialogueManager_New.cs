using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class DialogueManager_New : MonoBehaviour
{

    public static DialogueManager_New Instance;

    // note for dialogue - specific lines held by NPC, start and stop logic here
    public ConversationData currentConversation;
    public int currentLineIndex = 0;

    public UIManager_New UIManager;
    [SerializeField] private DialogueRunner dialogueRunner;
    //[SerializeField] private LineAdvancer lineAdvancer;
    [SerializeField] private CustomLineAdvancer customLineAdvancer;
    //[SerializeField] private InputActionReference advanceAction;

    [Header("UI")]
    [SerializeField] private GameObject TrustBar;

    [Header("Bools")]
    //private Queue<string> lines = new Queue<string>();
    public bool isDialogueActive = false;
    private bool canAdvance = true; // help w typewriter effect -- always true for now
                                    // ^^ also can handle in another script, keep in mind for now
    private bool justStartedDialogue = false;
    public bool isPaused = false;

    private NPC_New currentNPC;

    [Header("Input")]//player inputs
    public ThirdPersonController thirdPersonController;

    public static event Action<float> OnTrustChange;
    public static event Action<float> OnTrustBarActivated;
    public static event Action<NPCProfile_New> OnDialogueActivated;


    private void Awake()
    {

        // Singleton pattern (simple version); avoid duplicates
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialogueRunner.AddCommandHandler("activate_favor", ActivateFavor);
        dialogueRunner.AddCommandHandler("met_player", MetPlayer);
        dialogueRunner.AddCommandHandler("activate_selling", ActivateSelling);
        dialogueRunner.AddCommandHandler("update_trust", UpdateTrust);
        //dialogueRunner.AddCommandHandler<string>("set_node", SetNode);

        dialogueRunner.AddCommandHandler<string>(
            "set_node",
            (node) => SetNode(node)
        );

    }

    void Update()
    {

        if (justStartedDialogue)
        {
            justStartedDialogue = false;
            return;
        }

    }

    private void OnEnable()
    {
        //FavorManager.OnObjectiveComplete += SetNode;
    }

    private void OnDisable()
    {
        //FavorManager.OnObjectiveComplete += SetNode;
    }


    // start dialogue
    // Call this to start a conversation
    public void StartConversation(string dialogueNode, NPC_New npc) // old: (ConversationData conversation)
    {
        //Debug.Log("START CONVERSATION CALLED");
        //Debug.Log("Node: " + dialogueNode);


        if (string.IsNullOrEmpty(dialogueNode))
        {
            Debug.LogError("Tried to start dialogue with NULL or empty node!");
            return;
        }

        currentNPC = npc;

        if (currentNPC != null) 
        {
            OnDialogueActivated?.Invoke(npc.GetProfile());
        }
        //OnDialogueActivated?.Invoke(npc.GetProfile());

        UIManager.ShowDialoguePanel();  // turn MY panel on
        ToggleTrustPanel(npc == null);
        customLineAdvancer.OnDialogueStarted(); // MAY NOT BE NEEDED
        dialogueRunner.StartDialogue(dialogueNode); // let yarn spinner handle running dialogue TESTING

        //Debug.Log("StartDialogue finished.");


        isDialogueActive = true;
        justStartedDialogue = true;

        //change to Mouse Map
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }


    // ----------YARN COMMANDS----------
    //[YarnCommand("met_player")]
    public void MetPlayer()
    {
        currentNPC.GetProfile().SetMetPlayer(true);
    }

    //[YarnCommand("activate_favor")]
    public void ActivateFavor()
    {
        if (currentNPC == null)
        {
            Debug.LogError("No current NPC!");
            return;
        }

        FavorManager.Instance.StartFavor(currentNPC.GetProfile().GetCurrentFavor());
    }

    public void ActivateSelling()
    {
        // inventory big panel set active, use button set to sell
        // add yarn action to: InventoryUIController controller, toggleinventory

        // find a way to pause yarnspinner dialogue...
        // and resume it...

        customLineAdvancer.inputEnabled = false;


        InventoryUIController.Instance.IsSellingCheese();

        Debug.Log("Attempted to sell!");
        Debug.Log("Selling true? " + InventoryUIController.Instance.isSelling);

        InventoryUIController.Instance.ToggleInventory();
        InventoryUIController.Instance.OpenCheeseInventory();

        //at the end, call







    }

    public void ResumeDialogue()
    {

        //thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
        //advanceAction.action.Enable();
        customLineAdvancer.inputEnabled = true;
        //advanceAction.action.Enable();

        //thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;


    }


    public void UpdateTrust()
    {
        if (currentNPC == null)
        {
            Debug.LogError("No current NPC!");
            return;
        }

        currentNPC.GetProfile().IncreaseTrustLevel();
        OnTrustChange?.Invoke(currentNPC.GetProfile().GetTrustLevel());
    }

    public void SetNode(string newNode)
    {
        currentNPC.GetProfile().GetState().SetDialogueNode(newNode);
    }


    // end dialogue
    public void EndDialogue()
    {
        UIManager.HideDialoguePanel();
        ToggleTrustPanel(currentNPC == null);

        isDialogueActive = false;

        customLineAdvancer.OnDialogueComplete(); // MAY NOT NEED

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //Debug.Log("Conversation ended!");

        // CHANGE LOCATION LATER, MAKE A YARN COMMAND???
        // moved InventoryUIController.Instance.IsConsumingCheese(); back to use button


    }

    // ----------DIALOGUE UI----------
    private void ToggleTrustPanel(bool isEnforcer = false)
    {
        if (isEnforcer)
            return;
        

        if (TrustBar.activeSelf == false)
        {

            TrustBar.SetActive(true);

            OnTrustBarActivated?.Invoke(currentNPC.GetProfile().GetTrustLevel());

            return;
        }

        TrustBar.SetActive(false);

    }

    public void StartOptions()
    {
        UIManager_New.Instance.ShowOptionsPanel();
    }

    public void EndOptions()
    {
        UIManager_New.Instance.HideOptionsPanel();
    }



}
