using UnityEngine;

[CreateAssetMenu(menuName = "Cheese/Final Cheese Data")]
public class FinalResultCheese : ScriptableObject
{
    [Header("ID")]
    public string itemID; //for save system
    public string cheeseName;
    public CheeseRarity cheeseRarity;
    public CheeseType cheeseType;
    public StabilityLevel stability;

    public Sprite icon;

    [TextArea]
    public string description;

    public int basePrice;

    
    public int maxStack = 10;

    
}
public enum StabilityLevel
{
    Low,
    Medium,
    High
}
public enum CheeseRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
public enum CheeseType
{
    Upper,
    Downer
}