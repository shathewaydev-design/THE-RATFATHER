using UnityEngine;
using UnityEngine.UI;

public enum IngredientType
{
    Milk,
    Flavor,
    Additive
}

public class IngredientButton : MonoBehaviour
{
    public IngredientType type;
    public Image highlight;

    private RecipeMenuUI menu;

    void Start()
    {
        menu = GetComponentInParent<RecipeMenuUI>();
    }

    public void OnClick()
    {
        menu.SelectIngredient(this);
    }

    public void SetActive(bool active)
    {
        highlight.enabled = active;
    }
}