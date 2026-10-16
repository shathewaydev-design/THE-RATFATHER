using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ItemCategory
{
    Ingredient,
    Cheese,
    KeyItem
}
[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/CheeseIngredientData")]
public class CheeseIngredientData : ScriptableObject
{
    
    [Header("ID")]
    public string itemID; //for save system
    [Header("Item Info")]
    
    [TextArea] public string description;
    public string itemName;
    [Header("Visual")]
    
    public Sprite icon;
    [Header("Category")]
    public ItemCategory category;
    [Header("Item Info")]

    // Only relevant when category == Ingredient
    public IngredientRarity ingredientRarity;
    public IngredientType type;

    
    public string price;
    public int basePrice;

    [Header("Stacking")]
    public int maxStack = 10;
}
    public enum IngredientRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }
    public enum IngredientType
    {
        Milk,
        Flavor,
        Addictive
    }
    
