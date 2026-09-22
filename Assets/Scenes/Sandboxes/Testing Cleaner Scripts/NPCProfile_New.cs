using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPCProfile_New : ScriptableObject
{
    [SerializeField] private string description;
    [SerializeField] private Sprite portrait;

    [SerializeField] private string dialogueNode;

    [SerializeField] private int difficultyLevel;
    [SerializeField] private float trustLevel = 0;
    private float maxTrust = 100f;

    private int compFavors = 0;

    public float GetTrustLevel()
    {
        return trustLevel;
    }

    // for direct change of trust level IF npc needs to be
    // at a certainb level for progression purposes (not through standard gameplay)
    public void SetTrustLevel(float num)
    {
        trustLevel = num;
    }

    public float GetMaxTrust()
    {
        return maxTrust;
    }

    // normal way to calculate based oin difficulty level
    // of NPC
    public void IncreaseTrustLevel()
    {
        if (difficultyLevel == 1)
        {
            trustLevel += 50;
        } 
        else if (difficultyLevel == 2)
        {
            trustLevel += 25;
        } 
        else if (difficultyLevel == 3)
        {
            trustLevel += 15;
        } 
        else
        {
            trustLevel = 100;
        }

    }

    public void DecreaseTrustLevel()
    {
        if (difficultyLevel == 1)
        {
            trustLevel -= 15;
        }
        else if (difficultyLevel == 2)
        {
            trustLevel -= 25;
        }
        else if (difficultyLevel == 3)
        {
            trustLevel -= 50;
        }
        else
        {
            return;
        }

    }



}
