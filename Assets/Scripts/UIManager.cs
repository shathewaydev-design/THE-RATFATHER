using StarterAssets;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    public static UIManager Instance;


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

    public GameObject businessLog;
    public TextMeshProUGUI emptyTag;
    public GameObject soldLog;
    public GameObject recruitLog;  

    public GameObject sellingPanel;
    public GameObject sellButton;
    private List<Button> sellButtons = new List<Button>();
    private int sellSelectedIndex = 0;
    private bool justOpenedSellScreen = false;

    private List<TextMeshProUGUI> activeObjectives;

    private List<Button> optionButtons = new List<Button>();

    public ThirdPersonController player;

    private int selectedIndex = 0;
    private Action<int> currentCallback;


    void Awake()
    {
        // Singleton pattern (simple version); avoid duplicates
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        currentObjectiveText = Instantiate(objectivePrefab, objectivePanel.transform);
        activeObjectives = new List<TextMeshProUGUI>();
    }


    void Update()
    {
        if (justOpenedSellScreen)
        {
            justOpenedSellScreen = false;
            return;
        }

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

        // SELLING SCREEN IS OPEN
        if (sellingPanel.activeSelf && sellButtons.Count > 0)
        {

            // selection
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                sellSelectedIndex = (sellSelectedIndex + 1) % sellButtons.Count;
                UpdateSellHighlight();
            }


            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                sellSelectedIndex = (sellSelectedIndex - 1 + sellButtons.Count) % sellButtons.Count;
                UpdateSellHighlight();
            }

            // Press selected button
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                 sellButtons[sellSelectedIndex].onClick.Invoke();
            }

            // leave sell screen
            if (Keyboard.current.bKey.wasPressedThisFrame) // might need to move out so even if there is nothing to sell, can still close screen
            {
                ToggleSellScreen();
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

    void UpdateSellHighlight()
    {
        for (int i = 0; i < sellButtons.Count; i++)
        {
            var colors = sellButtons[i].colors;
            colors.normalColor = (i == sellSelectedIndex) ? Color.red : Color.white;
            sellButtons[i].colors = colors;
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
        var newObj = Instantiate(objectivePrefab, objectivePanel.transform);
        newObj.text = obj;

        activeObjectives.Add(newObj);
    }


    public void ClearObjectives()
    {
        foreach (var obj in activeObjectives)
        {
            Destroy(obj.gameObject);
        }

        activeObjectives.Clear();
    }


    public void RemoveObjective(string objText)
    {
        for (int i = activeObjectives.Count - 1; i >= 0; i--)
        {
            if (activeObjectives[i] != null && activeObjectives[i].text == objText)
            {
                Destroy(activeObjectives[i].gameObject);
                activeObjectives.RemoveAt(i);
            }
        }
    }

    public void ToggleSellScreen()
    {

        bool isActive = sellingPanel.activeSelf;

        if (isActive)
        {
            player.enabled = true;
            sellingPanel.SetActive(false);
            ClearSellUI();
            return;
        }

        player.enabled = false;

        Debug.Log("opening sell screen! Press S to sell and B to close");
        sellingPanel.SetActive(true);

        justOpenedSellScreen = true;
        PopulateSellUI();

    }

    void ClearSellUI()
    {
        foreach (Transform child in sellingPanel.transform)
        {
            Destroy(child.gameObject);
        }

        sellButtons.Clear();
    }

    void PopulateSellUI()
    {
        foreach (var pair in InventorySystem.Instance.Inventory)
        {
            var cheese = pair.Key;
            int count = pair.Value;

            if (cheese.ingredientName == "Low Quality Cheese") // temp filter
            {
                for (int i = 0; i < count; i++)
                {
                    var newButton = Instantiate(sellButton, sellingPanel.transform);

                    var text = newButton.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = "sell: " + cheese.ingredientName;


                    var button = newButton.GetComponent<Button>();
                    sellButtons.Add(button);

                    var cheeseCopy = cheese; // prevent closure bug

                    button.onClick.AddListener(() => GameManager.Instance.SellItem(cheeseCopy));

                }
            }
        }

        sellSelectedIndex = 0;
        UpdateSellHighlight();
    }

    public void ToggleLog()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            if (businessLog.activeSelf) 
            {
                businessLog.SetActive(false);
                return;
            }
            businessLog.SetActive(true);
        }
    }

    public void AddToSoldLog(NPCProfile npc)
    {
        var newTag = Instantiate(emptyTag, soldLog.transform);
        newTag.text = npc.characterName + ": example selling desc";

        //activeObjectives.Add(newObj);
    }

    public void AddToRecruitLog(NPCProfile npc)
    {
        var newTag = Instantiate(emptyTag, recruitLog.transform);
        newTag.text = npc.characterName + ": example rec. desc";

        //activeObjectives.Add(newObj);
    }




}
