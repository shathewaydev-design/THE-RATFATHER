using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cheese Template", menuName = "Scriptable Objects/Cheese Recipe Template")]
public class CheeseRecipeTemplate : ScriptableObject
{
    public string cheeseName;

    public Sprite icon;

    public List<CheeseIngredientData> requiredIngredients;

    public FinalResultCheese resultCheese;

    public StabilityLevel stability;
}