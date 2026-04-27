using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IngredientButton : MonoBehaviour
{
    public IngredientType type;
    public Image highlight;

    private RecipeMenuUI menu;
    //public CheeseIngredientData ingredientData;
    public InventorySlot inventorySlot;//this locally stores inventory slot data.
    //public CheeseIngredientData IngredientData => inventorySlot.itemData;
    private InventorySystem inventorySystem;
    private bool isSelected = false;

    void Start()
    {
        //Setup(inventorySlot);
        inventorySystem = InventorySystem.Instance;
    }
    public void Initialize(RecipeMenuUI recipeMenu)
    {
        menu = recipeMenu;
    }
    // public void Setup(InventorySlot slot)
    // {
    //     inventorySlot = slot;
        //ingredientData = slot.itemData;
    //     // slot = inventorySlot;
    //     // type = inventorySlot.itemData.type;

    //}
    public void OnClick()
    {
        menu.SelectIngredient(this);
        if (isSelected)//deselect
        {
            isSelected = false;
            ClearSelectedIngredient();
        }
        else//select
        {
            isSelected = true;
            
            ReplaceIngredientOfSameType();//if the player already has a selected ingredient of the same type, replace it with the new one
            CacheSelectedIngredient();
        }
        
    }
    public void CacheSelectedIngredient()
    {
        if(!CookingManager.Instance.selectedIngredients.Contains(this))
        {
            CookingManager.Instance.selectedIngredients.Add(this);
        }
    }
    public void ClearSelectedIngredient()
    {
        CookingManager.Instance.selectedIngredients.Remove(this);
    }
    public void RemoveIngredient()
    {
        inventorySystem.RemoveIngredientItem(this.inventorySlot.itemData, 1);
    }
    private void ReplaceIngredientOfSameType()
    {
        List<IngredientButton> selected =
            CookingManager.Instance.selectedIngredients;

        IngredientButton ingredientToRemove = null;

        foreach (IngredientButton ingredient in selected)
        {
            if (ingredient.type == type)
            {
                ingredientToRemove = ingredient;
                break;
            }
        }

        if (ingredientToRemove != null)
        {
            //ingredientToRemove.isSelected = false;

            selected.Remove(ingredientToRemove);
        }
    }

    public void SetActive(bool active)
    {
        highlight.enabled = active;
    }
    
}