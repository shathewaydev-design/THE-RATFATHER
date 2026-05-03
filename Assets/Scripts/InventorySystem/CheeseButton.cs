using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CheeseButton : MonoBehaviour
{
    //public IngredientType type;
    public Image highlight;

    public CheeseInventorySlot cheeseInventorySlot;
    //this locally stores cheeseinventory slot data/cheese data.

    private InventorySystem inventorySystem;
    private bool isSelected = false;

    void Start()
    {
        inventorySystem = InventorySystem.Instance;
    }
    
    public void OnClick()
    {
        if (isSelected)//deselect
        {
            isSelected = false;
            //ClearSelectedCheese();
        }
        else//select
        {
            isSelected = true;
            //ClearSelectedCheese();//if the player already has a selected cheese, 
            //CacheSelectedCheese();//replace it with the new one
        }
        if (InventoryUIController.Instance.selectedCheeses.Contains(this))
        {
            InventoryUIController.Instance.selectedCheeses.Remove(this);
            //if player click on the same cheese button, deselect it and remove from selected cheeses list; 
            //SetActive(false);
        }
        else
        {
            InventoryUIController.Instance.selectedCheeses.Clear(); // if single select
            InventoryUIController.Instance.selectedCheeses.Add(this);//cacheSelectedCheese
            //SetActive(true);
        }

        InventoryUIController.Instance.UpdateUseButton();
    }
    // public void CacheSelectedCheese()
    // {
    //     if(!InventoryUIController.Instance.selectedCheeses.Contains(this))
    //     {
    //         InventoryUIController.Instance.selectedCheeses.Add(this);
    //         InventoryUIController.Instance.UpdateUseButton();//initialize the use button with the selected cheese;this
    //     }
    // }
    // public void ClearSelectedCheese()
    // {
    //     InventoryUIController.Instance.selectedCheeses.Clear();
    
    // public void RemoveIngredient()
    // {
    //     inventorySystem.RemoveIngredientItem(this.inventorySlot.itemData, 1);
    // }
    // private void ReplaceCheeseOfSameSelection()
    // {
    //     List<CheeseButton> selected = CookingManager.Instance.selectedCheeses;

    //     CheeseButton cheeseToRemove = null;

    //     foreach (CheeseButton cheese in selected)
    //     {
    //         if (cheese.type == type)
    //         {
    //             cheeseToRemove = cheese;
    //             break;
    //         }
    //     }

    //     if (cheeseToRemove != null)
    //     {
    //         //cheeseToRemove.isSelected = false;

    //         selected.Remove(cheeseToRemove);
    //     }
    // }

    public void SetActive(bool isSelected)
    {
        highlight.enabled = isSelected;
    }
    
}