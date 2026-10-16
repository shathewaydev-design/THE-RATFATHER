using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SlumsLevelManager : MonoBehaviour
{
    public NPCProfile_New franProf;
    private int franTutorialStage = 0;

    public List<GameObject> levelNPCS;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
}
