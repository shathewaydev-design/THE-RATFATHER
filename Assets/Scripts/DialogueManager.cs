using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    // note for dialogue - specific lines held by NPC, start and stop logic here
  
    private Queue<string> lines = new Queue<string>();
    private bool isDialogueActive = false;
    private bool canAdvance = true; // help w typewriter effect -- always true for now
    // ^^ also can handle in another script, keep in mind for now


    // start dialogue
    public void StartDialogue(DialogueData dialogue)
    {
        isDialogueActive = true;
        lines.Clear(); // clear out any previous lines

        foreach (string line in dialogue.lines) // loop through each line in the dialoge data
        {
            lines.Enqueue(line); // add line from dialogue data to queue
        }

        // after each line is added to queue, display next line
        DisplayNextLine();
    }

    // display next line in dialogue
    public void DisplayNextLine()
    {
        if (!isDialogueActive) // if dialogue isnt active, don't display
            return;

        if (lines.Count == 0) // end dialogue if count is 0
        {
            EndDialogue();
            return;
        }

        string currentLine = lines.Dequeue();

        // Replace this with UI later
        Debug.Log(currentLine);
    }

    // end dialogue
    void EndDialogue()
    {
        isDialogueActive = false;
        Debug.Log("Dialogue ended.");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && canAdvance && Keyboard.current.eKey.wasPressedThisFrame) // Input.GetKeyDown(KeyCode.E) is legacy...
        {
            DisplayNextLine();
        }
    }
}
