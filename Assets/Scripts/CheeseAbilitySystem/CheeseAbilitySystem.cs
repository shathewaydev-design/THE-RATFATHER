using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CheeseAbilitySystem : MonoBehaviour
{
    //this script is for the deciding what ability to give player based on the cheese they consume.
    public static CheeseAbilitySystem Instance;
    

    //[SerializeField] private TMP_Text useButtonText; // Reference to the Text component of the button
    [SerializeField] private HypervigilanceScript hypervigilanceScript;
    [SerializeField] private DownerScript downerScript;
    [Header("Cheese Ability Settings")]
    [SerializeField] private int upperCheeseLimit = 3;
    [SerializeField] private int currentNumberUpperCheeseTaken = 0;
    [SerializeField] private int downerCheeseLimit = 3;
    [SerializeField] private int currentNumberDownerCheeseTaken = 0;



    private void Awake()
    {
        Instance = this;
    }

    // void Start()
    // {
    //     // Set the initial text of the button based on the current state
    
    // }
 
    
    
    public bool TryConsumeCheese(FinalResultCheese finalResultCheese)
    {
        if (finalResultCheese == null)
            return false;

        switch (finalResultCheese.cheeseType)
        {
            case CheeseType.Upper:

                if (currentNumberUpperCheeseTaken >= upperCheeseLimit)
                {
                    NotificationUIController.Instance.ShowNotification("I can't eat more Upper Cheese today.");
                    return false;
                }

                hypervigilanceScript.ActivateHypervigilance();
                currentNumberUpperCheeseTaken++;
                return true;


            case CheeseType.Downer:

                if (currentNumberDownerCheeseTaken >= downerCheeseLimit)
                {
                    NotificationUIController.Instance.ShowNotification("I can't eat more Downer Cheese today.");
                    return false;
                }

                downerScript.AddTime();
                currentNumberDownerCheeseTaken++;
                return true;
        }

        return false;
    }
    public void ResetDailyCheeseLimits()
    {
        currentNumberUpperCheeseTaken = 0;
        currentNumberDownerCheeseTaken = 0;

        Debug.Log("Daily cheese consumption limits reset!");
    }

    
    
    
}