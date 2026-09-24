using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager_New : MonoBehaviour
{
    public static UIManager_New Instance;

    public TextMeshProUGUI speakerText;     // UI element for the speaker's name
    public TextMeshProUGUI dialogueText;    // UI element for the line text
    public GameObject dialoguePanel;   // parent panel for dialogue and name

    public GameObject optionsPanel;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        
    }

    public void ShowDialoguePanel()
    {
        if (dialoguePanel.activeSelf)
            return;
        dialoguePanel.SetActive(true);
    }

    public void HideDialoguePanel()
    {
        if (!dialoguePanel.activeSelf)
            return;
        dialoguePanel.SetActive(false);
    }

    public void ShowOptionsPanel()
    {
        if (optionsPanel.activeSelf)
            return;
        optionsPanel.SetActive(true);
    }

    public void HideOptionsPanel()
    {
        if (!optionsPanel.activeSelf)
            return;
        optionsPanel.SetActive(false);
    }

    // Show a single line
    public void SetSpeaker(string speaker)
    {
        speakerText.text = speaker;
    }

    public void SetText(string text)
    {
        dialogueText.text = text;
    }
    
    
}
