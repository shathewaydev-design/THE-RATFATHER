using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yarn.Unity;


[CreateAssetMenu(fileName = "NPCProfile_New", menuName = "Scriptable Objects/NPCProfile_New")]
public class NPCProfile_New : ScriptableObject
{

    public static event Action<float> OnTrustLevelChange;

    [SerializeField] private string description;
    [SerializeField] private Sprite portrait;
    [SerializeField] private string introNode;
    private NPCState_New state;
    [SerializeField] private int difficultyLevel;
    [SerializeField] private int numOfFavors;
    private float maxTrust = 100f;

    private void Awake()
    {
        state = new NPCState_New();

        state.hasMetPlayer = false;
        state.dialogueNode = "";
        state.activeFavor = null;
        state.activeObjective = null;
        state.trustLevel = 0f;
        state.compFavors = 0;

    }

    public NPCState_New GetState()
    {
        return state;
    }

    public string GetIntroNode()
    {
        return introNode;
    }


    public bool GetHasMetPlayer()
    {
        return state.hasMetPlayer;
    }

    [YarnCommand("met_player")]
    public void MetPlayer()
    {
        state.hasMetPlayer = false;
    }

    public float GetTrustLevel()
    {
        return state.trustLevel;
    }

    // for direct change of trust level IF npc needs to be
    // at a certainb level for progression purposes (not through standard gameplay)
    public void SetTrustLevel(float num)
    {
        state.trustLevel = num;
    }

    public float GetMaxTrust()
    {
        return maxTrust;
    }

    // normal way to calculate based oin difficulty level
    // of NPC
    public void IncreaseTrustLevel()
    {
        OnTrustLevelChange?.Invoke(state.trustLevel);

        if (difficultyLevel == 1)
        {
            state.trustLevel += 50;
        } 
        else if (difficultyLevel == 2)
        {
            state.trustLevel += 25;
        } 
        else if (difficultyLevel == 3)
        {
            state.trustLevel += 15;
        } 
        else
        {
            state.trustLevel = 100;
        }

    }

    public void DecreaseTrustLevel()
    {
        OnTrustLevelChange?.Invoke(difficultyLevel);

        if (difficultyLevel == 1)
        {
            state.trustLevel -= 15;
        }
        else if (difficultyLevel == 2)
        {
            state.trustLevel -= 25;
        }
        else if (difficultyLevel == 3)
        {
            state.trustLevel -= 50;
        }
        else
        {
            return;
        }

    }

    public void IncreaseCompFavors()
    {
        state.compFavors++;

        if (state.compFavors >= numOfFavors)
        {
            state.compFavors = numOfFavors;
        }
    }

    public string GetCurrDialogueNode()
    {
        return state.CurrDialogueNode(this);
    }



}

[System.Serializable]
public class NPCState_New
{
    public bool hasMetPlayer;

    public string dialogueNode;

    public FavorState activeFavor; // one active favor PER NPC!!
    public FavorObjective activeObjective;

    public float trustLevel = 0;

    public int compFavors = 0;

    public string CurrDialogueNode(NPCProfile_New profile)
    {
        if (!hasMetPlayer)
        {
            dialogueNode = profile.GetIntroNode();
            return dialogueNode;
        }

        return "";
    }



}
