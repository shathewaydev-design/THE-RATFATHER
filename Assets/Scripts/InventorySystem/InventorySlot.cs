[System.Serializable]
public class InventorySlot
{
    public CheeseIngredientData itemData;
    public int quantity;

    public InventorySlot(CheeseIngredientData data, int amount)
    {
        itemData = data;
        quantity = amount;
    }
    public bool IsFull()
    {
        return quantity >= itemData.maxStack;
    }
}