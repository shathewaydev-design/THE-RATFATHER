using UnityEngine;

[CreateAssetMenu(fileName = "CheeseRequirement", menuName = "Scriptable Objects/CheeseRequirement")]
public class CheeseRequirement : QuestRequirement
{
    public FinalResultCheese cheese;
    public override bool CheckRequirement()
    {
        //throw new System.NotImplementedException();
        return InventorySystem.Instance.ContainsCheese(cheese);

    }
}
