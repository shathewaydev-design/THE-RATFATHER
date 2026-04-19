using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyReward", menuName = "Quests/Rewards/CurrencyReward")]
public class CurrencyReward : QuestReward
{
    public GameManager gameManager;
    public int currencyAddBy;
    public override void Apply()
    {
        throw new System.NotImplementedException();
        //gameManager.increaseCurrency(currencyAddBy);


    }
}
