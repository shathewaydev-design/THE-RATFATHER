using UnityEngine;

[CreateAssetMenu(menuName = "Cheese/Final Cheese Data")]
public class FinalResultCheese : ScriptableObject
{
    public string cheeseName;

    public Sprite icon;

    [TextArea]
    public string description;

    public int sellValue;

    public bool grantsDoubleJump;
    
    public int maxStack = 10;

    public StabilityLevel stability;
}
public enum StabilityLevel
{
    Low,
    Medium,
    High
}