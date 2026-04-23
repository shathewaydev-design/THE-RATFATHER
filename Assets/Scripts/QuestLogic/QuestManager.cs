using System;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public static QuestManager Instance;

    public DialogueManager dialogueManager;
    public UIManager UIManager;

    public bool questGiven = false;
    public bool questDone = false;

    public List<Quest> allQuests;
    public List<Quest> activeQuests;
    public List<Quest> compQuests;

    private Dictionary<Quest, QuestState> questStates;

    void Awake()
    {

        // Singleton pattern (simple version); avoid duplicates
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        questStates = new Dictionary<Quest, QuestState>();

        foreach (Quest quest in allQuests)
        {
            QuestState state = new QuestState
            {
                quest = quest,
                isCompleted = false,
                isActive = false,
                progress = 0
            };

            questStates.Add(quest, state);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            Quest quest = activeQuests[i];

            if (!questStates[quest].isCompleted)
            {
                CheckRequirement(quest);
            }
        }

        for (int i = compQuests.Count - 1; i >= 0; i--)
        {
            Quest quest = compQuests[i];

            if (!questStates[quest].isActive && CheckIfTurnedIn(quest))
            {
                foreach (QuestReward reward in quest.rewards)
                {
                    reward.Apply();
                    questStates[quest].hasGivenRewards = true;
                }
            }
        }

    }

    public Quest GetRelevantQuest(List<Quest> npcQuests)
    {
        // TURN-IN (completed but not turned in yet)
        foreach (var quest in npcQuests)
        {
            var state = questStates[quest];

            if (state.isCompleted && !state.hasGivenRewards)
            {
                return quest;
            }
        }

        // IN PROGRESS
        foreach (var quest in npcQuests)
        {
            if (questStates[quest].isActive)
            {
                return quest;
            }
        }

        // AVAILABLE (NOT completed yet)
        foreach (var quest in npcQuests)
        {
            var state = questStates[quest];

            if (!state.isCompleted && !state.isActive)
            {
                return quest;
            }
        }

        return null;
    }

    bool CheckIfTurnedIn(Quest quest)
    {

        if (dialogueManager.currentConversation == null)
            return false;

        if (!dialogueManager.isDialogueActive)
            return false;

        questStates[quest].hasTurnedIn = true;
        return dialogueManager.currentConversation == quest.questCompleted;

    }

    public void UpdateObjective(Quest quest, int newIndex)
    {
        var state = questStates[quest];

        state.objectiveIndex = newIndex;

        string newObjective = quest.objectives[newIndex];

        UIManager.SetObjective(newObjective);


    }

    public ConversationData UpdateDialogue(Quest quest)
    {
        if (compQuests.Contains(quest) && questStates[quest].hasTurnedIn)
            return quest.questOver;

        if (compQuests.Contains(quest))
            return quest.questCompleted;

        if (activeQuests.Contains(quest))
            return quest.questQuestioned;
       // fallback
        return quest.questIntro;
    }

    public void CheckRequirement(Quest quest) // options: 1, 2, or 3 stages for a quest
                                              // I don't want NPC given quests to be too complicated
    {
        if (quest.requirements.Count <= 0)
        {
            return;
        }

        if (quest.requirements.Count == 1)
        {
            if (quest.requirements[0].CheckRequirement() && !compQuests.Contains(quest))
            {
                ChangeQuestCompleted(quest);
                return;
            }
        }
        else if (quest.requirements.Count == 2)
        {
            if (quest.requirements[0].CheckRequirement() && !compQuests.Contains(quest))
            {
                ChangeQuestProgress(quest);
                UpdateObjective(quest, 1);
                return;

            }
            else if (quest.requirements[1].CheckRequirement() && !compQuests.Contains(quest))
            {
                ChangeQuestCompleted(quest);
                return;
            }
        }
        else
        {
            if (quest.requirements[0].CheckRequirement() && !compQuests.Contains(quest))
            {
                ChangeQuestProgress(quest);
                UpdateObjective(quest, 1);
                return;

            }
            else if (quest.requirements[1].CheckRequirement() && !compQuests.Contains(quest))
            {
                ChangeQuestProgress(quest);
                UpdateObjective(quest, 2);
                return;
            }
            else if (quest.requirements[2].CheckRequirement() && !compQuests.Contains(quest))
            {
                ChangeQuestCompleted(quest);
                return;
            }

        }

    }


    public void ChangeQuestActive(Quest quest)
    {
        if (questStates[quest].isActive)
        {
            questStates[quest].isActive = false;
            activeQuests.Remove(quest);
            return;
        }

        questStates[quest].isActive = true;
        activeQuests.Add(quest);
        UIManager.SetObjective(quest.objectives[0]);
    }

    public void ChangeQuestCompleted(Quest quest) // can help with UI later
    {
        questStates[quest].isCompleted = true;
        if (questStates[quest].isActive)
        {
            UIManager.RemoveObjective(quest.objectives[questStates[quest].objectiveIndex]);
            ChangeQuestActive(quest);
        }

        compQuests.Add(quest);
    }

    public void ChangeQuestProgress(Quest quest)
    {
        questStates[quest].progress++;
    }
}
