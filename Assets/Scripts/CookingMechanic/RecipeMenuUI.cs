using UnityEngine;
using UnityEngine.UI;

public class RecipeMenuUI : MonoBehaviour
{
    public Button cookButton;

    private IngredientButton selectedMilk;
    private IngredientButton selectedFlavor;
    private IngredientButton selectedAddictive;
    private CheeseRecipeTemplate currentRecipe;//for caching recipe and check if player has selected correct ingredients

    public void SelectIngredient(IngredientButton button)
    {
        switch (button.type)
        {
            case IngredientType.Milk:
                SetSelection(ref selectedMilk, button);
                //button.Setup(inventorySlot.itemData);
                Debug.Log("selectedMilk is "+selectedMilk);
                break;

            case IngredientType.Flavor:
                SetSelection(ref selectedFlavor, button);
                //button.Setup(inventorySlot.itemData);
                Debug.Log("selected Flavor is "+selectedFlavor);
                break;

            case IngredientType.Addictive:
                SetSelection(ref selectedAddictive, button);
                //button.Setup(inventorySlot.itemData);
                Debug.Log("selectedAddictive is "+selectedAddictive);
                break;
        }

        CheckReady();
    }

    void SetSelection(ref IngredientButton current, IngredientButton newButton)//if ingredientButton is same type, select the new one
    {
        if (current == newButton)
        {
            current.SetActive(false);
            current = null;
            return;
        }

        if (current != null)
            current.SetActive(false);

        current = newButton;
        current.SetActive(true);
    }

    void CheckReady()
    {
        
        bool ready = selectedMilk != null && selectedFlavor != null && selectedAddictive != null;
        cookButton.interactable = ready;

    }

    public void OnCookPressed()
    {
        currentRecipe = CookingManager.Instance.GetMatchingRecipe(CookingManager.Instance.selectedIngredients);//cache this recipe
        if(currentRecipe == null)
        {
            NotificationUIController.Instance.ShowNotification("Invalid recipe");
            return;
        }
        // Valid recipe
        CookingManager.Instance.StartPourMilk();
        cookButton.interactable = false;

        ResetSelections();
    }

    void Start ()
    {
        cookButton.interactable = false;
    }
    public void ResetSelections()//reset all buttons
{
    if (selectedMilk != null)
    {
        selectedMilk.SetActive(false);
        selectedMilk = null;
    }

    if (selectedFlavor != null)
    {
        selectedFlavor.SetActive(false);
        selectedFlavor = null;
    }

    if (selectedAddictive != null)
    {
        selectedAddictive.SetActive(false);
        selectedAddictive = null;
    }

    cookButton.interactable = false;
}
}