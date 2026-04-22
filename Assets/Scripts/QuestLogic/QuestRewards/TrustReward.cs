using UnityEngine;

[CreateAssetMenu(fileName = "TrustReward", menuName = "Quests/Rewards/TrustReward")]
public class TrustReward : QuestReward
{
    public override void Apply()
    {
        //throw new System.NotImplementedException();
        Debug.Log("+10 Trust for this Rat!");
    }
}
