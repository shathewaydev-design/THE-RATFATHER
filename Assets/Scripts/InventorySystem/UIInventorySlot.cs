using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    public Image icon;
    // public TextMeshProUGUI quantityText;
    public Text quantityText;
    public Text ingredientNameText;
    public IngredientButton ingredientButton;
    private RecipeMenuUI recipeMenuUI;
    private Button button;
    private IngredientButton ingredientButtonComponent;

    void Start()
    {
        button = GetComponent<Button>();
        ingredientButtonComponent = GetComponent<IngredientButton>();
    }
    public void Initialize(RecipeMenuUI menu)
    {
        recipeMenuUI = menu;
    }
    public void SetSlot(InventorySlot slot)//grab data from the inventory slot and set the UI elements accordingly
    {
        ingredientButton.Initialize(recipeMenuUI);
        icon.sprite = slot.itemData.icon;
        ingredientNameText.text = slot.itemData.ingredientName;

        quantityText.text = slot.quantity.ToString();
        ingredientButton.type = slot.itemData.type;
        // if (slot == null)
        // {
        //     Debug.LogError("Slot is NULL");
        //     return;
        // }

        // if (slot.itemData == null)
        // {
        //     Debug.LogError("slot.itemData is NULL");
        //     return;
        // }
        // ingredientButtonComponent.Setup(slot);
    }
    
}