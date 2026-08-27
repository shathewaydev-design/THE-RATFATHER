using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public int requiredTrust;
    public List<Quest> requiredCompletedQuests;

    //public QuestManager questManager;

    public ConversationData questIntro; // dialogue when npc gives quest
    public ConversationData questQuestioned; // dialogue when player is in middle of npc's quest
    public ConversationData questCompleted; // dialogue once quest is completed
    public ConversationData questOver;

    public List<QuestRequirement> requirements;
    public List<string> objectives;
    //public List<CheeseIngredientData> keyItems; // distinguish between ingredients and other items later!!
    public List<QuestReward> rewards;



}
