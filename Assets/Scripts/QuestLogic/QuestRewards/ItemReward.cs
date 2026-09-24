using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Rewards/ItemReward")]
public class ItemReward : QuestReward
{

    public CheeseIngredientData item;
    //public bool applied = false;

    public override void Apply()
    {
        //throw new System.NotImplementedException();
        InventorySystem.Instance.AddIngredientItem(item);
        Debug.Log("ITEM ADDED");
        applied = true;
        //Debug.Log("GIVEN cheese ingredient and milk removed!");

    }
}
