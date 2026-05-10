using StarterAssets;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance;

    // note for dialogue - specific lines held by NPC, start and stop logic here
    public ConversationData currentConversation;
    public int currentLineIndex = 0;

    public UIManager UIManager;
    //private Queue<string> lines = new Queue<string>();
    public bool isDialogueActive = false;
    private bool canAdvance = true; // help w typewriter effect -- always true for now
                                    // ^^ also can handle in another script, keep in mind for now
    private bool justStartedDialogue = false;
    public bool isPaused = false;
    
    [Header("Input")]//player inputs
    public ThirdPersonController thirdPersonController;


    private void Awake()
    {

        // Singleton pattern (simple version); avoid duplicates
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

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

            ShowCurrentLine();
        }
    }


    // start dialogue
    // Call this to start a conversation
    public void StartConversation(ConversationData conversation)
    {
        if (conversation == null)
        {
            Debug.LogError("Tried to start conversation with NULL data!");
            return;
        }

        justStartedDialogue = true;
        //change to Mouse Map
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


        isDialogueActive = true;

        currentConversation = conversation;
        currentLineIndex = 0;

        UIManager.ShowDialoguePanel();  // turn panel on
        ShowCurrentLine();
    }

    // display next line in dialogue
    public void ShowCurrentLine()
    {
        if (currentLineIndex >= currentConversation.lines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentConversation.lines[currentLineIndex];

        // Show line in the UI
        UIManager.SetSpeaker(line.speaker);
        UIManager.SetText(line.text);

        // Show options if they exist
        if (line.options != null && line.options.Count > 0)
        {
            UIManager.ShowOptions(line.options, OnOptionSelected);
        }
        else
        {
            UIManager.HideOptions();
            // automatically advance when player clicks "Next" button (optional)
        }

    }


    // Called when player clicks an option
    public void OnOptionSelected(int selectedOptionIndex)
    {
        //UIManager.HideOptions();
        DialogueOption selected = currentConversation.lines[currentLineIndex].options[selectedOptionIndex];
        selected.GiveQuest();
        selected.Recruit();

        if (selected.openSellScreen)
        {
            selected.Sell();
            PauseDialogue();   // pause before anything else
            return;            // no advancing dialogue
        }

        if (selected.endConversation) 
        {
            EndDialogue(); // panel hides, flow stops
            return;
        }

        if (selected.nextLineIndex >= 0)
            currentLineIndex = selected.nextLineIndex;
        else
            currentLineIndex++;

        ShowCurrentLine();
    }

    // end dialogue
    void EndDialogue()
    {
        UIManager.HideDialoguePanel();
        currentConversation = null;
        currentLineIndex = 0;

        isDialogueActive = false;

        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Conversation ended!");
    }


    void Update()
    {

        if (isPaused)
        {
            return;

        }


        if (justStartedDialogue)
        {
            justStartedDialogue = false;
            return;
        }

        if (currentConversation == null)
            return;

        if (currentLineIndex < currentConversation.lines.Count &&
            currentConversation.lines[currentLineIndex].endConversation &&
             Keyboard.current.eKey.wasPressedThisFrame)
        {
            EndDialogue();
            return;
        }

        // Only advance with E if no options are active
        if ((currentConversation.lines.Count > currentLineIndex) &&
            (currentConversation.lines[currentLineIndex].options == null ||
             currentConversation.lines[currentLineIndex].options.Count == 0))
        {
            if (thirdPersonController.mouseClick.IsPressed())//Keyboard.current.eKey.wasPressedThisFrame
            {
                currentLineIndex++;
                ShowCurrentLine();
            }
        }
    }
}
