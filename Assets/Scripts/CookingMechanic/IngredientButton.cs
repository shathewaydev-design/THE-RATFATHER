using UnityEngine;
using UnityEngine.UI;


public class IngredientButton : MonoBehaviour
{
    public IngredientType type;
    public Image highlight;

    private RecipeMenuUI menu;
    //public CheeseIngredientData ingredientData;
    public InventorySlot inventorySlot;
    //public CheeseIngredientData IngredientData => inventorySlot.itemData;

    void Start()
    {
        
    }
    public void Initialize(RecipeMenuUI recipeMenu)
    {
        menu = recipeMenu;
    }
    // public void Setup(InventorySlot slot)
    // {
    //     inventorySlot = slot;
    //     //ingredientData = slot.itemData;
    //     // slot = inventorySlot;
    //     // type = inventorySlot.itemData.type;

    // }
    public void OnClick()
    {
        menu.SelectIngredient(this);
        //CookingManager.Instance.SelectIngredient(inventorySlot);
    }

    public void SetActive(bool active)
    {
        highlight.enabled = active;
    }
    
}