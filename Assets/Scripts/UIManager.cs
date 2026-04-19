using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;     // UI element for the speaker's name
    public TextMeshProUGUI dialogueText;    // UI element for the line text
    public GameObject dialoguePanel;   // parent panel for dialogue and name
    public GameObject optionsPanel;   // parent panel for buttons
    public Button optionButtonPrefab; // prefab for a single option

    public QuestManager questManager;
    public GameManager gameManger;

    public TextMeshProUGUI objectivePrefab;
    public GameObject objectivePanel;
    private TextMeshProUGUI currentObjectiveText;

    private List<Button> optionButtons = new List<Button>();

    private int selectedIndex = 0;
    private Action<int> currentCallback;


    void Awake()
    {
        currentObjectiveText = Instantiate(objectivePrefab, objectivePanel.transform);
    }


    void Update()
    {
        if (optionsPanel.activeSelf && optionButtons.Count > 0)
        {
            // Navigate options
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                selectedIndex = (selectedIndex + 1) % optionButtons.Count;
                UpdateButtonHighlight();
            }
            else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                selectedIndex = (selectedIndex - 1 + optionButtons.Count) % optionButtons.Count;
                UpdateButtonHighlight();
            }

            // Select option
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                currentCallback?.Invoke(selectedIndex);
            }
        }
    }

    private void UpdateButtonHighlight()
    {
        for (int i = 0; i < optionButtons.Count; i++)
        {
            ColorBlock colors = optionButtons[i].colors;
            colors.normalColor = (i == selectedIndex) ? Color.yellow : Color.white;
            optionButtons[i].colors = colors;
        }
    }



    public void ShowDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }

    public void HideDialoguePanel()
    {
        if (optionsPanel.activeSelf) 
        {
            optionsPanel.SetActive(false);
        }
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

    // Show player options (branching)
    public void ShowOptions(List<DialogueOption> options, Action<int> callback)
    {
        currentCallback = callback;

        // Clear old buttons
        foreach (Transform child in optionsPanel.transform)
            Destroy(child.gameObject);

        optionButtons.Clear();

        // Create new buttons
        for (int i = 0; i < options.Count; i++)
        {
            int index = i; // capture for closure
            Button button = Instantiate(optionButtonPrefab, optionsPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = options[i].text;
            button.onClick.AddListener(() => callback(index));
            optionButtons.Add(button); // add to list for highlighting and selecting
        }

        selectedIndex = 0;            // reset selection
        optionsPanel.SetActive(true); // highlight selection
    }
    public void HideOptions()
    {
        foreach (Transform child in optionsPanel.transform)
            Destroy(child.gameObject);
        optionButtons.Clear();
        optionsPanel.SetActive(false);
    }

    public void SetObjective(string obj)
    {
        currentObjectiveText.text = obj;
    }

    public void ClearObjective()
    {
        currentObjectiveText.text = "";
    }



}
