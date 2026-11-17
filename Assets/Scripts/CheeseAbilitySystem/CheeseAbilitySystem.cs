using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CheeseAbilitySystem : MonoBehaviour
{
    //this script is for the deciding what ability to give player based on the cheese they consume.
    public static CheeseAbilitySystem Instance;
    

    public CheeseButton selectedCheese;//ref to the cheese button 

    //[SerializeField] private TMP_Text useButtonText; // Reference to the Text component of the button
    [SerializeField] private HypervigilanceScript hypervigilanceScript;
    [SerializeField] private DownerScript downerScript;


    private void Awake()
    {
        Instance = this;
    }

    // void Start()
    // {
    //     // Set the initial text of the button based on the current state
    
    // }
    public void SelectCheese(CheeseButton cheese)
    {
        selectedCheese = cheese;
    }
    
    public void GivePlayerAbility()
    {
        if (selectedCheese == null)
            return;
        CheeseType type = selectedCheese.cheeseInventorySlot.finalCheeseData.cheeseType;
        switch (type)
        {
            case CheeseType.Upper:
                hypervigilanceScript.ActivateHypervigilance();
                break;

            case CheeseType.Downer:
                downerScript.AddTime();
                break;
        }
        selectedCheese = null; 
    }

    
    
    
}