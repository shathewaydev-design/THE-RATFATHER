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

    public bool endConversation; // does option end conversation?

    public void GiveQuest()
    {
        if (assignQuest)
        {
            QuestManager.Instance.ChangeQuestActive(quest);
            Debug.Log("Quest assigned!");
        }
        return;
    }

}