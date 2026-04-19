using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Rewards/ItemReward")]
public class ItemReward : QuestReward
{

    public CheeseIngredientData item;

    public override void Apply()
    {
        //throw new System.NotImplementedException();
        InventorySystem.Instance.RemoveItem(item);
        //Debug.Log("GIVEN cheese ingredient and milk removed!");

    }
}
