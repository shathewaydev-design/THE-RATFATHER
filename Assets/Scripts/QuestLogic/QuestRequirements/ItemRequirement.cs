using UnityEngine;

[CreateAssetMenu(fileName = "ItemRequirement", menuName = "Scriptable Objects/ItemRequirement")]
public class ItemRequirement : QuestRequirement
{
   // public QuestManager questManager;
   // public InventorySystem inventorySystem;

    public CheeseIngredientData item;
    public override bool CheckRequirement()
    {
        //throw new System.NotImplementedException();
        return InventorySystem.Instance.Inventory.ContainsKey(item);
    }
}
