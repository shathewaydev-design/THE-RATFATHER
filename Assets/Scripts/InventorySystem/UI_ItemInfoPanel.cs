using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ItemInfoPanel : MonoBehaviour
{
    public GameObject itemInfoPanelGameObject;

    public Text nameText;
    public Text rarityText;
    public Text descriptionText;

    // Optional fields (hidden for ingredients)
    public GameObject basePriceContainer;
    public Text basePriceText;

    public GameObject effectContainer;
    public Text effectText;

    public GameObject stabilityContainer;
    public Text stabilityText;
    // Optional fields (hidden for cheese)
    public GameObject ingredientTypeContainer;
    public Text ingredientTypeText;

    public void ShowIngredient(CheeseIngredientData data)
    {
        itemInfoPanelGameObject.SetActive(true);

        nameText.text = data.ingredientName;
        rarityText.text = data.rarity.ToString();
        descriptionText.text = data.description;
        ingredientTypeText.text = data.type.ToString();

        //Show fields relevant to ingredients
        ingredientTypeContainer.SetActive(true);

        // Hide fields not used by ingredients
        basePriceContainer.SetActive(false);
        effectContainer.SetActive(false);
        stabilityContainer.SetActive(false);
    }
    public void ShowCheese(FinalResultCheese data)
    {
        itemInfoPanelGameObject.SetActive(true);

        nameText.text = data.cheeseName;
        rarityText.text = data.rarity.ToString();
        descriptionText.text = data.description;
        basePriceText.text = data.basePrice.ToString();
        //Show fields relevant to cheese
        effectContainer.SetActive(true);
        basePriceContainer.SetActive(true);
        stabilityContainer.SetActive(true);
        // Hide fields not used by cheese
        ingredientTypeContainer.SetActive(false);
        
        
    }
    public void Hide()
    {
        itemInfoPanelGameObject.SetActive(false);
    }
}