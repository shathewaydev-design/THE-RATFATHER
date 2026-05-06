using UnityEngine;

[CreateAssetMenu(fileName = "NPCRequirement", menuName = "Scriptable Objects/NPCRequirement")]
public class NPCRequirement : QuestRequirement
{
    public NPCProfile npc;
    public override bool CheckRequirement()
    {
        //throw new System.NotImplementedException();

        return npc.hasMet;

    }
}
