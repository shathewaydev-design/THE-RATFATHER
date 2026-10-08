using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class UseButton : MonoBehaviour
{

    public static Func<FinalResultCheese, bool> OnSellAttempt;

    //this script is for the use button that appears when player selects a cheese; 
    // it allows player to consume the cheese or sell it to NPC
    //public static UseButton Instance;
    public CheeseButton cheeseButtonRef;//ref to the cheese button 
    public IngredientButton ingredientButtonRef;//ref to the cheese button 

    [SerializeField] private TMP_Text useButtonText; // Reference to the Text component of the button

    [Header("Selling/Consuming")]
    public bool isSelling = false;
    public bool isConsuming = true;

    // private void Awake()//this is necessary to avoid bugs
    // {
    //     if (Instance == null)
    //         Instance = this;
    //     else
    //         Destroy(gameObject);

    // }

    // void Start()
    // {
    //     // Set the initial text of the button based on the current state
    //     UpdateButtonText();
    // }
    
    public void UseButtonInteracted()
    {

        // if (NPCDialogueManager.Instance.IsTalkingToNPC())
        // {
        //     SellCheese(selectedCheese);
        // }
        // else
        // {
        //     ConsumeCheese(selectedCheese);
        // }
        
        if(isSelling)
        {
            SellCheese();
        }
        else
        {
            ConsumeCheese();
        }
        
    }
    // public void IsSellingCheese()
    // {
    //     isSelling = true;
    //     isConsuming = false;
    //     UpdateButtonText();
    // }
    // public void IsConsumingCheese()
    // {
    //     isSelling = false;
    //     isConsuming = true;
    //     UpdateButtonText();
    // }
    void ConsumeCheese()
    {
        InventorySystem inventorySystem = InventorySystem.Instance;
        if (cheeseButtonRef != null && cheeseButtonRef.cheeseInventorySlot != null)
        {
            CheeseInventorySlot cheeseSlot = cheeseButtonRef.cheeseInventorySlot;
            if (cheeseSlot.quantity > 0)
            // Consume the cheese (can add more logic here, e.g., apply effects to the player)
            {
                Debug.Log("Used cheese: " + cheeseSlot.finalCheeseData.cheeseRarity);
                InventoryUIController.Instance.ApplyEffect();
                WithdrawalMechanic.Instance.EatCheese(cheeseSlot.finalCheeseData.cheeseRarity);
                inventorySystem.RemoveFinalCheese(cheeseSlot.finalCheeseData, 1);
                // After using the cheese, you might want to refresh the UI or perform other actions    
            }
            
            InventoryUIController.Instance.useButton.SetActive(false);
            
        }
    }
    void SellCheese()
    {
        InventorySystem inventorySystem = InventorySystem.Instance;
        if (cheeseButtonRef != null && cheeseButtonRef.cheeseInventorySlot != null)
        {
            CheeseInventorySlot cheeseSlot = cheeseButtonRef.cheeseInventorySlot;
            if (cheeseSlot.quantity > 0)
            // Consume the cheese (can add more logic here, e.g., apply effects to the player)
            {

                // check npc preference by triggering event
                bool canSell = OnSellAttempt.Invoke(cheeseSlot.finalCheeseData);

                if (canSell)
                {
                    inventorySystem.RemoveFinalCheese(cheeseSlot.finalCheeseData, 1); // successful sell? remove cheese

                    int newPrice = cheeseSlot.finalCheeseData.basePrice;

                    // if there are selling buffs, can set here w new int!
                    // new func to grab from npc_prof?


                    inventorySystem.AddToCurrency(newPrice + 100); // succesful sell? give player money
                    Debug.Log("Added to currency: " + (cheeseSlot.finalCheeseData.basePrice + 100));

                    Debug.Log("Sold cheese: " + cheeseSlot.finalCheeseData.cheeseName);

                }
                else
                {
                    Debug.Log("NPC will not purchase this cheese!");

                    // now access dialogue manager, at least to trigger a new fail node
                }

                //cheeseSlot.finalCheeseData.stability;
                //the stability enum is from FinalResultCheese script that lives on CheeseInventory

                //InventoryUIController.Instance.ApplyEffect();
                //DialogueManager_New.Instance.ResumeDialogue(true);//continue dialogue


                //UIManager.Instance.ToggleSellScreen();
                //Cursor.visible = true;
                //Cursor.lockState = CursorLockMode.None;


                //inventorySystem.RemoveFinalCheese(cheeseSlot.finalCheeseData, 1);

                // After using the cheese, you might want to refresh the UI or perform other actions    
            }

            
            InventoryUIController.Instance.IsConsumingCheese();

            InventoryUIController.Instance.ToggleInventory();

            InventoryUIController.Instance.useButton.SetActive(false);
            

        }
    }
    public void UpdateButtonText()
    {
        if (isSelling)
        {
            useButtonText.text = "Sell";
        }
        else if (isConsuming)
        {
            useButtonText.text = "Eat";
        }
    }

    
    
    
    
}