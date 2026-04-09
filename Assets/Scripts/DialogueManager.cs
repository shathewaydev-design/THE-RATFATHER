using StarterAssets;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    // note for dialogue - specific lines held by NPC, start and stop logic here
    public ConversationData currentConversation;
    public int currentLineIndex = 0;

    public UIManager UIManager;
    public ThirdPersonController player;
    //private Queue<string> lines = new Queue<string>();
    private bool isDialogueActive = false;
    private bool canAdvance = true; // help w typewriter effect -- always true for now
                                    // ^^ also can handle in another script, keep in mind for now


    // start dialogue
    // Call this to start a conversation
    public void StartConversation(ConversationData conversation)
    {
        isDialogueActive = true;
        player.enabled = false;

        currentConversation = conversation;
        currentLineIndex = 0;

        UIManager.ShowDialoguePanel();      // turn panel on
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
        DialogueOption selected = currentConversation.lines[currentLineIndex].options[selectedOptionIndex];

        if (selected.endConversation && currentLineIndex == currentConversation.lines.Count)
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
        player.enabled = true;

        Debug.Log("Conversation ended!");
    }


    void Update()
    {
        if (currentConversation == null)
            return;

        if (currentConversation.lines[currentLineIndex].endConversation && Keyboard.current.eKey.wasPressedThisFrame)
        {
            EndDialogue();
            return;
        }

        // Only advance with E if no options are active
        if ((currentConversation.lines.Count > currentLineIndex) &&
            (currentConversation.lines[currentLineIndex].options == null ||
             currentConversation.lines[currentLineIndex].options.Count == 0))
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                currentLineIndex++;
                ShowCurrentLine();
            }
        }
    }
}
