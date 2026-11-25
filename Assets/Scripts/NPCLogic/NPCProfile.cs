using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NPCProfile", menuName = "Scriptable Objects/NPCProfile")]
public class NPCProfile : ScriptableObject
{

    public string npcName;
    public NPCState npcState = new NPCState();

    public CheeseType typePreference;
    public Quality qualityPreference;
    public int basePrice; // what an npc will offer per cheese before other calculations

    public string introNode;
    public string customerNode;
    public string noRequestNode; // thinking this could be lore building dialogue
    public string noLongerCustomerNode;

}
