using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyReward", menuName = "Quests/Rewards/CurrencyReward")]
public class CurrencyReward : QuestReward
{
    public GameManager gameManager;
    public int currencyAddBy;
    //public bool applied = false;


    public override void Apply()
    {
        applied = true;
        throw new System.NotImplementedException();
        //gameManager.increaseCurrency(currencyAddBy);
        


    }
}
