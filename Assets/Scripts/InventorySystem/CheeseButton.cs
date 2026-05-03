using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CheeseButton : MonoBehaviour
{
    //public IngredientType type;
    public Image highlight;

    //private RecipeMenuUI menu;
    public CheeseInventorySlot cheeseInventorySlot;
    //this locally stores cheeseinventory slot data/cheese data.

    private InventorySystem inventorySystem;
    private bool isSelected = false;
    [SerializeField] private GameObject useButton;//this button is for consuming the cheese or selling

    void Start()
    {
        inventorySystem = InventorySystem.Instance;
    }
    // public void Initialize(RecipeMenuUI recipeMenu)
    // {
    //     menu = recipeMenu;
    // }
    public void OnClick()
    {
        //menu.SelectIngredient(this);
        if (isSelected)//deselect
        {
            isSelected = false;
            useButton.SetActive(false);
            ClearSelectedCheese();
        }
        else//select
        {
            isSelected = true;
            useButton.SetActive(true);
            ClearSelectedCheese();//if the player already has a selected cheese, 
            CacheSelectedCheese();//replace it with the new one
        }
        
    }
    public void CacheSelectedCheese()
    {
        if(!CookingManager.Instance.selectedCheeses.Contains(this))
        {
            CookingManager.Instance.selectedCheeses.Add(this);
        }
    }
    public void ClearSelectedCheese()
    {
        CookingManager.Instance.selectedCheeses.Clear();
    }
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