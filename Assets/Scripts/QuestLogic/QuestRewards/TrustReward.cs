using UnityEngine;

[CreateAssetMenu(fileName = "TrustReward", menuName = "Quests/Rewards/TrustReward")]
public class TrustReward : QuestReward
{

    //public bool applied = false;

    public override void Apply()
    {
        //throw new System.NotImplementedException();
        applied = true;
        Debug.Log("+10 Trust for this Rat!");

    }
}
