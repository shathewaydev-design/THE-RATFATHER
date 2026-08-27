using UnityEngine;

[CreateAssetMenu(fileName = "QuestReward", menuName = "Quests/Rewards/QuestReward")]
public abstract class QuestReward : ScriptableObject
{
    // the one thing every reward must be able to do
    public abstract void Apply(); // not sure what I want as a perameter yet
    public bool applied;
}
