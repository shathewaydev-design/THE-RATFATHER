using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UseButton : MonoBehaviour
{
    //this script is for the use button that appears when player selects a cheese; 
    // it allows player to consume the cheese or sell it to NPC
    public CheeseButton cheeseButtonRef;//ref to the cheese button 

    void Start()
    {
        
    }
    
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
        ConsumeCheese();
        
    }
    void ConsumeCheese()
    {
        InventorySystem inventorySystem = InventorySystem.Instance;
        if (cheeseButtonRef != null && cheeseButtonRef.cheeseInventorySlot != null)
        {
            CheeseInventorySlot cheeseSlot = cheeseButtonRef.cheeseInventorySlot;
            if (cheeseSlot.quantity > 0)
            // Consume the cheese (can add more logic here, e.g., apply effects to the player)
            {
                Debug.Log("Used cheese: " + cheeseSlot.finalCheeseData.cheeseName);
                InventoryUIController.Instance.ApplyEffect();
                inventorySystem.RemoveFinalCheese(cheeseSlot.finalCheeseData, 1);
                // After using the cheese, you might want to refresh the UI or perform other actions    
            }
            
            InventoryUIController.Instance.useButton.SetActive(false);
            
        }
    }

    
    
    
    
}