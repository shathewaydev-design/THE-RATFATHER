using UnityEngine;
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
    //distinction:
    //PokerBossData contains information that should remain the same.
    //PokerBossState contains information that changes while playing.
    //For example, the boss's starting confidence might always be 100,
    //but his current confidence might be 60.
    [SerializeField] private float currentBossConfidence;
    [SerializeField] private BossDifficulty currentDifficulty;
    [SerializeField] private int dosesRemaining;//doses left to use for this boss
    [SerializeField] private float currentPlayerConfidence;

    private void Initialize(PokerBossData pokerBossData)
    {
        //Sets initial confidence and doses
    }
    private void DecreaseBossConfidence(float amount)
    {
        
    }
    
    private void DecreasePlayerConfidence(float amount)
    {
        
    }
    private void UpdateDifficulty()
    {
        //Updates difficulty based on confidence 
    }
    private void HasDefeatedBoss()
    {
        //Checks whether boss confidence is zero
    }
    private void PlayerDefeated()
    {
        //Checks whether player confidence is zero
    }








}