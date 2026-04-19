using UnityEngine;

[CreateAssetMenu(fileName = "QuestRequirement", menuName = "Scriptable Objects/QuestRequirement")]
public abstract class QuestRequirement : ScriptableObject
{
    public abstract bool CheckRequirement();
}
