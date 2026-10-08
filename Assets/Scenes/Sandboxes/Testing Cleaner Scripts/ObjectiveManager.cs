using StarterAssets;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private InputActionReference openObjectives;
    public bool canOpenObjectives = true;
    //private int index = 0;

    [SerializeField] private List<ObjectivesPage> pages = new List<ObjectivesPage>();
    //[SerializeField] private List<ObjectiveVisual> objs = new List<ObjectiveVisual>(); // may not be needed
    private ObjectivesPage currPage;
    [SerializeField] private int currPageIndex;

    [Header("UI")]
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private GameObject fullFavorDisplayPrefab;

    [SerializeField] private TextMeshProUGUI favorText;
    [SerializeField] private TextMeshProUGUI objectiveTextPrefab;

    [Header("Input")]//player inputs
    public ThirdPersonController thirdPersonController;

    private void Awake()
    {
        ObjectivesPage mainObj = new ObjectivesPage(); // set somewhere else?
        objectivePanel.SetActive(false);

        mainObj.favorMain = "Main Objective";
        pages.Add(mainObj);

        currPage = pages[0];

    }

    private void OnEnable()
    {
        FavorManager.OnFavorActivated += AddPage;
        FavorManager.OnObjectiveComplete += AddObjective;
    }

    private void OnDisable()
    {
        FavorManager.OnFavorActivated -= AddPage;
        FavorManager.OnObjectiveComplete -= AddObjective;
    }

    void Update()
    {
        if (canOpenObjectives && openObjectives.action.WasPressedThisFrame())
        {
            ToggleObjectivePanel();
        }
    }

    public void ToggleObjectivePanel()
    {
        if (objectivePanel != null)
        {
            if (!objectivePanel.activeSelf)
            {
                objectivePanel.SetActive(true);

                currPageIndex = 0;
                ClearObjectives();
                SetAndDisplayPage();

                //change to Mouse Map
                thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                return;
            }

            objectivePanel.SetActive(false);

            //change to Player Map
            thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            return;
        }

        Debug.Log("No objective panel set!!!");

    }

    // methods below -> logic goal:
    // be able to add objective when one is complete.
    // strikethrough objective when said objective is complete
    // remove all objectives when entire favor is complete
    // also, set name of favor (entire main goal of objectives)
    // arrows to go through main objective, and other favors

    public void SetFavorName(string words)
    {
        favorText.text = words;
    }

    public void AddPage(FavorState favorstate)
    {
        // when a new favor is added for player (trigger via event?)
        ObjectivesPage newPage = new ObjectivesPage();

        newPage.favorMain = favorstate.favor.name;
        newPage.objectives.Add(favorstate.favor.objectives[0].description);

        pages.Add(newPage);

    }

    public void AddObjective(FavorState favorstate)
    {

        foreach (ObjectivesPage page in pages) 
        {
            if (favorstate.favor.name.Equals(page.favorMain))
            {
                if (favorstate.isCompleted)
                {
                    return;
                }

                if (favorstate.currentObjective >= favorstate.favor.objectives.Count)
                {
                    //RemovePage(currPageIndex);
                    return;
                }


                page.objectives.Add(favorstate.favor.objectives
                    [favorstate.currentObjective].description);

            }

        }

    }

    public void StrikeThroughObjective()
    {
        // for now, change opacity of text?
        // set through event!
    }

    public void RemovePage(int index)
    {
        pages.RemoveAt(index);
    }

    public void CyclePage()
    {
        if(pages.Count == 1)
            return;
        

        if (currPageIndex == pages.Count - 1)
        {
            currPageIndex = 0;
            // handle
            currPage = pages[currPageIndex];
            SetAndDisplayPage();
            return;
        }

        currPageIndex++;
        currPage = pages[currPageIndex];
        SetAndDisplayPage();


    }

    private void SetAndDisplayPage()
    {
        favorText.text = currPage.favorMain;

        int i = 0;
        foreach (string obj in currPage.objectives)
        {
            TextMeshProUGUI newText = Instantiate(objectiveTextPrefab, 
                fullFavorDisplayPrefab.transform);

            newText.text = currPage.objectives[i];
            i++;
        }

    }

    private void ClearObjectives()
    {
        foreach (ObjectivesPage page in pages)
        {
            foreach (Transform child in fullFavorDisplayPrefab.transform)
            {
                Destroy(child.gameObject);
            }

        }
    }

    public void OnArrowClick()
    {
        if (pages.Count <= 0)
            return;

        ClearObjectives();
        CyclePage();

    }


    public class ObjectivesPage
    {
        public string favorMain = "";
        public List<string> objectives = new List<string>();
    }

   

}
