using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheeseDatabase",
    menuName = "Database/Cheese Database")]
public class CheeseDatabase : ScriptableObject
{
    public List<FinalResultCheese> allIngredients;

    public FinalResultCheese GetCheeseByID(string id)
    {
        return allIngredients.Find(
            ingredient => ingredient.itemID == id
        );
    }
}