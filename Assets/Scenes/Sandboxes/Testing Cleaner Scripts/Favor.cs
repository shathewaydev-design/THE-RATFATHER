using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Favor", menuName = "Scriptable Objects/Favor")]
public class Favor : ScriptableObject
{
    public string favorName;
    public string favorID;
    public string description;

    public List<FavorObjective> objectives;
    public List<FavorReward> rewards;
}

public enum FavorObjectiveType
{
    ObtainItem,
    ReachLocation,
    TalkToNPC,
    TradeItem
}

[System.Serializable]
public class FavorObjective
{
    public string description;
    public string npcDialogue; // node for the NPC who assigned favor
    public FavorObjectiveType type;
    public string targetID; // try to use for cleaner script (later, for now use below)

    public CheeseIngredientData targetItem;
    public FinalResultCheese targetCheese;
    // eventually, misc. item
    public bool toBeRemoved; // should be removed after objective complete? or favor complete?

    public string targetLocationID;
    public NPCProfile_New targetNPC;

}

public enum FavorRewardType
{
    trustReward,
    currencyReward,
    cheeseReward,
    ingredientReward
}


[System.Serializable]
public class FavorReward
{
    public FavorRewardType type;

    public CheeseIngredientData ingredient;
    public FinalResultCheese cheese;
    public int currenyAmount;



}



[System.Serializable]
public class FavorState
{
    public Favor favor;
    public int currentObjective;
    public bool isCompleted;
}
