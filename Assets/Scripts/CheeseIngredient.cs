using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Cheese Ingredient", menuName = "Scriptable Objects/CheeseIngredientData")]
public class CheeseIngredientData : ScriptableObject
{
    public string ingredientName;
    public string rarity;
    public string price;
    public Sprite icon;
    [TextArea] public string description;
}
