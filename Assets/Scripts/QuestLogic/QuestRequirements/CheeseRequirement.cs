using UnityEngine;

[CreateAssetMenu(fileName = "CheeseRequirement", menuName = "Scriptable Objects/CheeseRequirement")]
public class CheeseRequirement : QuestRequirement
{
    public CheeseData cheese;
    public override bool CheckRequirement()
    {
        //throw new System.NotImplementedException();

        return true;
    }
}
