using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NPCProfile", menuName = "Scriptable Objects/NPCProfile")]
public class NPCProfile : ScriptableObject
{
    public string characterName;
    public string trait;
    public string description; // include type of cheese they like, and if recruitable, reward rate

    // need to add type of cheese(s) they like >> also need cheese types as well as a way to check if in inventory.

    public Image pfp;

    public int trustDifficulty;

    public bool recruited;
    public bool hasMet = false;

    public List<Quest> quests;
    public List<string> availability;

    // WORK ON UI TO DISPLAY

}
