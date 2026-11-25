using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public enum BossDifficulty
{
    Easy,
    Medium,
    Hard
}
public class PokerBossState : MonoBehaviour
{

    //store information that changes during the encounter.
    //boss confidence, player confidence
    //distinction:
    //PokerBossData contains information that should remain the same.
    //PokerBossState contains information that changes while playing.
    //For example, the boss's starting confidence might always be 100,
    //but his current confidence might be 60.
    private PokerBossData currentBossData;
    public float currentBossConfidence;
    [SerializeField] private BossDifficulty currentDifficulty;
    [SerializeField] private int dosesRemaining;//doses left to use for this boss
    public float currentPlayerConfidence;
    [SerializeField] private float bossDamagePerSuccess;//damage to boss
    [SerializeField] private float playerDamagePerSuccess;//damage to player
    public float BossConfidence => currentBossConfidence;

    public BossDifficulty CurrentDifficulty => currentDifficulty;
    public int DosesRemaining => dosesRemaining;

    public event Action<float, float> OnConfidenceChanged;
    public event Action<int> OnDoseCountChanged;
    public event Action<string> OnBossChanged;
    

    private void NotifyStateChanged()
    {
        OnConfidenceChanged?.Invoke(currentBossConfidence, currentPlayerConfidence);

        OnDoseCountChanged?.Invoke(dosesRemaining);
    }


    public void Initialize(PokerBossData pokerBossData)
    {
        //Sets initial confidence and doses
        currentBossData = pokerBossData;
        currentBossConfidence = pokerBossData.startingBossConfidence;
        currentPlayerConfidence = pokerBossData.startingPlayerConfidence;
        bossDamagePerSuccess = pokerBossData.bossDamagePerSuccess;
        playerDamagePerSuccess = pokerBossData.playerDamagePerSuccess;
        dosesRemaining = pokerBossData.availableDoses;

        UpdateDifficulty();
        NotifyStateChanged();
        OnBossChanged?.Invoke(currentBossData.bossName);
    }
    public void LoadBoss(PokerBossData bossData)
    {
        currentBossData = bossData;
        currentBossConfidence = bossData.startingBossConfidence;

        UpdateDifficulty();
        NotifyStateChanged();
        OnBossChanged?.Invoke(currentBossData.bossName);
    }
    public void DecreaseBossConfidence()
    {
        currentBossConfidence -= bossDamagePerSuccess;
        currentBossConfidence = Mathf.Max(currentBossConfidence, 0f);

        UpdateDifficulty();
        NotifyStateChanged();
    }
    
    public void DecreasePlayerConfidence()
    {
        currentPlayerConfidence -= playerDamagePerSuccess;
        currentPlayerConfidence = Mathf.Max(currentPlayerConfidence, 0f);

        NotifyStateChanged();
    }
    private void UpdateDifficulty()
    {
        //Updates difficulty based on confidence 
        if (currentBossConfidence > 66f)
        {
            currentDifficulty = BossDifficulty.Easy;
        }
        else if (currentBossConfidence > 33f)
        {
            currentDifficulty = BossDifficulty.Medium;
        }
        else
        {
            currentDifficulty = BossDifficulty.Hard;
        }

    }
    // private void HasDefeatedBoss()
    // {
    //     //Checks whether boss confidence is zero
    //     if(currentBossConfidence <= 0)
    //     {
            
    //     }
    // }
    // private void PlayerDefeated()
    // {
    //     //Checks whether player confidence is zero
    //     if(currentPlayerConfidence <= 0)
    //     {
            
    //     }
    // }
    public bool TryUseDose()
    {
        if (dosesRemaining <= 0)
        {
            Debug.Log("No doses remaining!");
            return false;
        }

        dosesRemaining--;

        Debug.Log("Doses remaining: " + dosesRemaining);

        return true;
    }
    

}