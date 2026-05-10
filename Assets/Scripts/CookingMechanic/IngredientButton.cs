using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IngredientButton : MonoBehaviour
{
    public IngredientType type;
    public Image highlight;

    private RecipeMenuUI menu;
    public InventorySlot inventorySlot;//this locally stores inventory slot data/ingredient data.

    private InventorySystem inventorySystem;
    public bool isSelected = false;

    void Start()
    {
        inventorySystem = InventorySystem.Instance;
    }
    public void Initialize(RecipeMenuUI recipeMenu)
    {
        menu = recipeMenu;
    }
    public void OnClick()
    {
        menu.SelectIngredient(this);
        if (isSelected)//deselect
        {
            isSelected = false;
            // HIDE INFO PANEL
            InventoryUIController.Instance.HideIngredientInfo();
            ClearSelectedIngredient();
        }
        else//select
        {
            isSelected = true;
            // SHOW INFO PANEL
            InventoryUIController.Instance.ShowIngredientInfo(inventorySlot.itemData);
            ReplaceIngredientOfSameType();//if the player already has a selected ingredient of the same type, replace it with the new one
            CacheSelectedIngredient();
        }
        
    }
    public void ResetSelectionBool()
    {
        isSelected = false;
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