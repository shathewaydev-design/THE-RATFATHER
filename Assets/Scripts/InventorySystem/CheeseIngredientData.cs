using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Cheese Ingredient", menuName = "Scriptable Objects/CheeseIngredientData")]
public class CheeseIngredientData : ScriptableObject
{
    
    [Header("ID")]
    public string itemID; //for save system
    [Header("Ingredient Info")]
    
    [TextArea] public string description;
    public string ingredientName;
    [Header("Visual")]
    public string price;
    public Sprite icon;
    [Header("Gameplay")]
    public IngredientRarity rarity;
    // public IngredientType type;

    [Header("Economy")]
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
    // public enum IngredientType
    // {
    //     Milk,
    //     Flavor,
    //     Addictive
    // }
    
