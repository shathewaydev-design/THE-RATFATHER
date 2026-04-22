using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConversationData", menuName = "Scriptable Objects/ConversationData")]
public class ConversationData : ScriptableObject
{
    public List<DialogueLine> lines;

}

[System.Serializable]
public class DialogueLine
{
    public NPCProfile speaker; // the character speaking
    public string text;        // what they say
    public List<DialogueOption> options; // optional choices

    public bool recruitRat; // does this option recruit a rat?
    public bool openSellScreen; // does this open the sell screen?

    public bool endConversation; // ends conversation if true
}

[System.Serializable]
public class DialogueOption
{
    public string text;       // player option text
    public int nextLineIndex; // which line to jump to (-1 = next)

    //public QuestManager questManager;
    public bool assignQuest; // does this option give a quest?
    public Quest quest; // quest assigned to option if needed

    public NPCProfile npc;

    public bool openSellScreen; // does this open the sell screen?
    public bool endConversation; // does option end conversation?

    public bool recruitRat; // does this option recruit a rat?

    public void GiveQuest()
    {
        if (assignQuest)
        {
            QuestManager.Instance.ChangeQuestActive(quest);
            Debug.Log("Quest assigned!");
        }
        return;
    }

    public void Recruit()
    {
        if (recruitRat)
        {
            GameManager.Instance.RecruitRat(npc);
           
        }
    }

    public void Sell()
    {
        if (openSellScreen)
        {
            UIManager.Instance.ToggleSellScreen();
            // ui manager open screen
        }

    }

}