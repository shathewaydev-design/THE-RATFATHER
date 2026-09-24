using StarterAssets;
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



    //private Queue<string> lines = new Queue<string>();
    public bool isDialogueActive = false;
    private bool canAdvance = true; // help w typewriter effect -- always true for now
                                    // ^^ also can handle in another script, keep in mind for now
    private bool justStartedDialogue = false;
    public bool isPaused = false;

    private NPC_New currentNPC;

    [Header("Input")]//player inputs
    public ThirdPersonController thirdPersonController;


    private void Awake()
    {

        // Singleton pattern (simple version); avoid duplicates
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialogueRunner.AddCommandHandler("activate_favor", ActivateFavor);
        dialogueRunner.AddCommandHandler("met_player", MetPlayer);
        //dialogueRunner.AddCommandHandler<string>("set_node", SetNode);

        dialogueRunner.AddCommandHandler<string>(
            "set_node",
            (node) => SetNode(node)
        );



    }

    private void OnEnable()
    {
        //FavorManager.OnObjectiveComplete += SetNode;
    }

    private void OnDisable()
    {
        //FavorManager.OnObjectiveComplete += SetNode;
    }



    public void PauseDialogue()
    {

        isPaused = true;
    }

    public void ResumeDialogue(bool advanceLine = false)
    {
        isPaused = false;

        if (currentConversation != null)
        {
            if (advanceLine)
            {
                currentLineIndex++; // advance once
            }

            //ShowCurrentLine();
        }
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

        UIManager.ShowDialoguePanel();  // turn MY panel on
        dialogueRunner.StartDialogue(dialogueNode); // let yarn spinner handle running dialogue TESTING

        //Debug.Log("StartDialogue finished.");


        isDialogueActive = true;
        justStartedDialogue = true;

        //change to Mouse Map
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

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

    public void SetNode(string newNode)
    {
        currentNPC.GetProfile().GetState().SetDialogueNode(newNode);
    }

    //public void SetNodeFromFavor(FavorState state)
    //{
    //    //currentNPC.GetProfile().GetState().
    //}


















    // display next line in dialogue
    //public void ShowCurrentLine()
    //{
    //    if (currentLineIndex >= currentConversation.lines.Count)
    //    {
    //        EndDialogue();
    //        return;
    //    }

    //    DialogueLine line = currentConversation.lines[currentLineIndex];

    //    // Show line in the UI
    //    UIManager.SetSpeaker(line.speaker);
    //    UIManager.SetText(line.text);

    //    // Show options if they exist


    //}


    // Called when player clicks an option
    //public void OnOptionSelected(int selectedOptionIndex)
    //{
    //    //UIManager.HideOptions();
    //    DialogueOption selected = currentConversation.lines[currentLineIndex].options[selectedOptionIndex];
    //    selected.GiveQuest();
    //    selected.Recruit();

    //    if (selected.openSellScreen)
    //    {
    //        selected.Sell();
    //        PauseDialogue();   // pause before anything else
    //        return;            // no advancing dialogue
    //    }

    //    if (selected.endConversation)
    //    {
    //        EndDialogue(); // panel hides, flow stops
    //        return;
    //    }

    //    if (selected.nextLineIndex >= 0)
    //        currentLineIndex = selected.nextLineIndex;
    //    else
    //        currentLineIndex++;

    //    //ShowCurrentLine();
    //}

    // end dialogue
    public void EndDialogue()
    {
        UIManager.HideDialoguePanel(); 
        // currentConversation = null;
        // currentLineIndex = 0;

        isDialogueActive = false;

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //Debug.Log("Conversation ended!");
    }

    public void StartOptions()
    {
        UIManager_New.Instance.ShowOptionsPanel();
    }

    public void EndOptions()
    {
        UIManager_New.Instance.HideOptionsPanel();
    }


    void Update()
    {

        //if (isPaused)
        //{
        //    return;

        //}


        if (justStartedDialogue)
        {
            justStartedDialogue = false;
            return;
        }

        //if (currentConversation == null)
        //    return;

        //if (currentLineIndex < currentConversation.lines.Count &&
        //    currentConversation.lines[currentLineIndex].endConversation &&
        //     Keyboard.current.eKey.wasPressedThisFrame) //Keyboard.current.eKey.wasPressedThisFrame
        //{
        //    EndDialogue();
        //    return;
        //}

        //// Only advance with E if no options are active
        //if ((currentConversation.lines.Count > currentLineIndex) &&
        //    (currentConversation.lines[currentLineIndex].options == null ||
        //     currentConversation.lines[currentLineIndex].options.Count == 0))
        //{
        //    if (Mouse.current.leftButton.wasPressedThisFrame)//Keyboard.current.eKey.wasPressedThisFrame // thirdPersonController.mouseClick.WasPressedThisFrame()
        //    {
        //        currentLineIndex++;
        //        // ShowCurrentLine();
        //    }
        //}
    }
}
