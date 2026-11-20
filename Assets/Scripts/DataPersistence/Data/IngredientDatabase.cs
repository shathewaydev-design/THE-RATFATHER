using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientDatabase",
    menuName = "Database/Ingredient Database")]
public class IngredientDatabase : ScriptableObject
{
    public List<CheeseIngredientData> allIngredients;

    public CheeseIngredientData GetIngredientByID(string id)
    {
        return allIngredients.Find(
            ingredient => ingredient.itemID == id
        );
    }
}