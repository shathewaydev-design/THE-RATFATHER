using StarterAssets;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    // new objective manager

    public static ObjectiveManager Instance;

    // EVENTS ---------------------------------------------->>>



    // VARIABLES ---------------------------------------------->>>

    private List<ObjectiveData> activeObjectives = new List<ObjectiveData>();
    public IReadOnlyList<ObjectiveData> ActiveObjectives => activeObjectives;

    // METHODS ---------------------------------------------->>>

    public void AddObjective(ObjectiveData obj) // CREATE AN OBJECTIVE DATA INT THE SCRIPT YOU CALL THIS METHOD!!!	
    {
        activeObjectives.Add(obj);
    }

    public void RemoveObjective(ObjectiveData obj)
    {
        activeObjectives.Remove(obj);
    }

    public void ClearActiveObjectives()
    {
        activeObjectives.Clear();
    }

    // Unity Methods --->>>

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() // used for testin for now!!!
    {
        //ObjectiveData obj1 = new ObjectiveData();
        //obj1.objectiveType = ObjectiveType.MainObjective;
        //obj1.objectiveName = "Talk to diddy blud";
        //obj1.objectiveDescription = "He wants to touch you...";
        //obj1.objectiveStatus = ObjectiveStatus.Incomplete;
        //obj1.isCall = true;
        //obj1.callNode = "node";

        //activeObjectives.Add(obj1);

        //ObjectiveData obj2 = new ObjectiveData();
        //obj2.objectiveType = ObjectiveType.SideObjective;
        //obj2.objectiveName = "Talk to diddy blud";
        //obj2.objectiveDescription = "He wants to touch you...";
        //obj2.objectiveStatus = ObjectiveStatus.Incomplete;
        //obj2.isCall = false;

        //activeObjectives.Add(obj2);


        //ObjectiveData obj3 = new ObjectiveData();

        //obj3.objectiveType = ObjectiveType.SideObjective;
        //obj3.objectiveName = "Talk to diddy blud";
        //obj3.objectiveDescription = "He wants to touch you...";
        //obj3.objectiveStatus = ObjectiveStatus.Incomplete;
        //obj3.isCall = false;

        //activeObjectives.Add(obj3);


        //ObjectiveData obj4 = new ObjectiveData();
        //obj4.objectiveType = ObjectiveType.SideObjective;
        //obj4.objectiveName = "Talk to diddy blud";
        //obj4.objectiveDescription = "He wants to touch you...";
        //obj4.objectiveStatus = ObjectiveStatus.Incomplete;
        //obj4.isCall = false;

        //activeObjectives.Add(obj4);

        //ObjectiveData obj5 = new ObjectiveData();
        //obj5.objectiveType = ObjectiveType.SideObjective;
        //obj5.objectiveName = "Talk to diddy blud";
        //obj5.objectiveDescription = "He wants to touch you...";
        //obj5.objectiveStatus = ObjectiveStatus.Incomplete;
        //obj5.isCall = false;

        //activeObjectives.Add(obj5);


    }





}
