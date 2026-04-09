using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCProfile", menuName = "Scriptable Objects/NPCProfile")]
public class NPCProfile : ScriptableObject
{
    public string characterName;
    public string trait;
    public int trustDifficulty;

    public List<Quest> quests;

    // WORK ON UI TO DISPLAY

}
