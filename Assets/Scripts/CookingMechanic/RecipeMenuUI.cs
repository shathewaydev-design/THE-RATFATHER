using UnityEngine;
using UnityEngine.UI;

public class RecipeMenuUI : MonoBehaviour
{
    public Button cookButton;

    private IngredientButton selectedMilk;
    private IngredientButton selectedFlavor;
    private IngredientButton selectedAdditive;

    public void SelectIngredient(IngredientButton button)
    {
        switch (button.type)
        {
            case IngredientType.Milk:
                SetSelection(ref selectedMilk, button);
                //Debug.Log("selectedMilk is "+selectedMilk);
                break;

            case IngredientType.Flavor:
                SetSelection(ref selectedFlavor, button);
                //Debug.Log("selected Flavor is "+selectedFlavor);
                break;

            case IngredientType.Additive:
                SetSelection(ref selectedAdditive, button);
                //Debug.Log("selectedAdditive is "+selectedAdditive);
                break;
        }

        CheckReady();
    }

    void SetSelection(ref IngredientButton current, IngredientButton newButton)
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
        if(selectedMilk != null && selectedFlavor != null && selectedAdditive != null)
        {    
            bool ready = true;
            cookButton.interactable = ready;
        }
    }

    public void OnCookPressed()
    {
        CookingManager.Instance.StartPourMilk();
        cookButton.interactable = false;
    }

    void Start ()
    {
        cookButton.interactable = false;
    }
}