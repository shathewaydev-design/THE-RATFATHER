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
                break;

            case IngredientType.Flavor:
                SetSelection(ref selectedFlavor, button);
                break;

            case IngredientType.Additive:
                SetSelection(ref selectedAdditive, button);
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
        bool ready = selectedMilk && selectedFlavor && selectedAdditive;
        cookButton.interactable = ready;
    }

    public void OnCookPressed()
    {
        CookingManager.Instance.StartPourMilk();
    }
}