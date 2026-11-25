using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokerBossData", menuName = "Scriptable Objects/PokerBossData")]
public class PokerBossData : ScriptableObject
{
    public string bossName;
    public float startingBossConfidence = 100f;
    public float startingPlayerConfidence = 100f;
    public float bossDamagePerSuccess = 20f;//damage to boss
    public float playerDamagePerSuccess = 10f;//damage to player
    [SerializeField] private float easyTellTime = 7f;
    [SerializeField] private float mediumTellTime = 4f;
    [SerializeField] private float hardTellTime = 2f;
    [SerializeField] private float easyReactionTime = 9f;
    [SerializeField] private float mediumReactionTime = 6f;
    [SerializeField] private float hardReactionTime = 3f;
    public float doseTellTime = 10f;
    [SerializeField] private float doseReactionTime = 11f;
    public float showdownTime = 10f;//time to show results
    public int availableDoses = 3;

    public float GetTellTime(BossDifficulty difficulty)
    {
        switch (difficulty)
        {
            case BossDifficulty.Easy:
                return easyTellTime;

            case BossDifficulty.Medium:
                return mediumTellTime;

            case BossDifficulty.Hard:
                return hardTellTime;

            default:
                return mediumTellTime;
        }
    }

    public float GetReactionTime(BossDifficulty difficulty)
    {
        switch (difficulty)
        {
            case BossDifficulty.Easy:
                return easyReactionTime;

            case BossDifficulty.Medium:
                return mediumReactionTime;

            case BossDifficulty.Hard:
                return hardReactionTime;

            default:
                return mediumReactionTime;
        }
    }
}