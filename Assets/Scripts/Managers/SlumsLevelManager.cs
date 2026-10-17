using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SlumsLevelManager : MonoBehaviour
{
    public NPCProfile_New franProf;
    private int franTutorialStage = 0;

    public List<GameObject> levelNPCS;

    [SerializeField] private List<ComicCutscene> InGameScenes;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CutsceneManager.Instance.currCutscene = InGameScenes[0];
        CutsceneManager.Instance.StartCutscene();


        ObjectiveManager.Instance.pages[0].objectives.Add("Talk to francheesqua.");
    }

    // Update is called once per frame
    void Update()
    {



        if (franTutorialStage == 0 && franProf.GetState().hasMetPlayer)
        {
            ObjectiveManager.Instance.pages[0].objectives.Remove("Talk to francheesqua.");
            ObjectiveManager.Instance.ClearObjectives(0);
            ObjectiveManager.Instance.pages[0].objectives.Add("Make a cheese. Grab several different ingredients.");

            franProf.GetState().SetDialogueNode("Francheesqua_Slums_Mid");

            franTutorialStage = 1;

        }

        if (franTutorialStage == 1 && InventorySystem.Instance.cheeseInventory.Count >= 1)
        {
            ObjectiveManager.Instance.pages[0].objectives.Remove("Make a cheese. Grab several different ingredients.");
            ObjectiveManager.Instance.ClearObjectives(0);
            ObjectiveManager.Instance.pages[0].objectives.Add("Talk to francheesqua again.");
            franProf.GetState().SetDialogueNode("Francheesqua_Slums_Done");
            franTutorialStage = 2;
        }

        if (franTutorialStage == 2)
        {
            ObjectiveManager.Instance.pages[0].objectives.Remove("Talk to francheesqua again.");
            ObjectiveManager.Instance.ClearObjectives(0);
            ObjectiveManager.Instance.pages[0].objectives.Add("Find other rats to sell to. Make more cheese.");
            ObjectiveManager.Instance.pages[0].objectives.Add("Make enough money to pay off debt before the end of the week.");
            franTutorialStage = 3;

        }

        EnableNPCs();

        LevelCompCheck();



    }

    private void EnableNPCs()
    {
        if (franTutorialStage == 3)
        {
            foreach (GameObject npc in levelNPCS)
            {
                npc.SetActive(true);
            }

            franTutorialStage++;
        }
    }

    bool transitionTriggered = false;
    private void LevelCompCheck()
    {
        if (!transitionTriggered)
        {
            if (InventorySystem.Instance.GetCurrency() >= 200 && !DialogueManager_New.Instance.isDialogueActive)
            {

                TransitionToSoldato();
                transitionTriggered = true;
            }
        }
        else
        {
            return;
        }
        
    }

    private void TransitionToSoldato()
    {
        // scene management after stubbed cutscene!
        // add stubbed cutscene manager to level!

        StubbedCutsceneManager.Instance.OpenCutscene(0);



    }
}
