using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager_New : MonoBehaviour
{
    public static UIManager_New Instance;


    public TextMeshProUGUI nameText;     // UI element for the speaker's name
    public TextMeshProUGUI dialogueText;    // UI element for the line text
    public GameObject dialoguePanel;   // parent panel for dialogue and name

    void Awake()
    {

    }


    void Update()
    {


    }


    public void ShowDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }

    public void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
    }



    // Show a single line
    public void SetSpeaker(NPCProfile speaker)
    {
        nameText.text = speaker.characterName; // or whatever your NPCProfile has
    }

    public void SetText(string text)
    {
        dialogueText.text = text;
    }
    
    
}
