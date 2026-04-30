using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CheeseInventorySlot : MonoBehaviour
{
    //Cheese Container Child.
    public Image icon;
    // public TextMeshProUGUI quantityText;
    public Text quantityText;
    public Text cheeseNameText;
    public CheeseButton cheeseButton;
    //private RecipeMenuUI recipeMenuUI;
    private Button button;
    //private IngredientButton ingredientButtonComponent;

    void Start()
    {
        button = GetComponent<Button>();
        //ingredientButtonComponent = GetComponent<IngredientButton>();
    }
    // public void Initialize(RecipeMenuUI menu)
    // {
    //     recipeMenuUI = menu;
    // }
    public void SetSlot(CheeseInventorySlot cheeseInventorySlot)
    //grab data from the inventory slot and display the UI elements accordingly
    {
        //ingredientButton.Initialize(recipeMenuUI);
        icon.sprite = cheeseInventorySlot.finalCheeseData.icon;
        cheeseNameText.text = cheeseInventorySlot.finalCheeseData.cheeseName;

        quantityText.text = cheeseInventorySlot.quantity.ToString();
        //ingredientButton.type = cheeseInventorySlot.finalCheeseData.type;
        cheeseButton.cheeseInventorySlot = cheeseInventorySlot;//pass the inventory slot data to the ingredient button 

    }
    
}