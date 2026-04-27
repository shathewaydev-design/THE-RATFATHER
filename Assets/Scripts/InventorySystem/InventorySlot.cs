[System.Serializable]
public class InventorySlot
{
    //this is where CheeseIngredientData is store. you can reference this to grab items out and remove items from
    public CheeseIngredientData itemData;
    public int quantity;

    public InventorySlot(CheeseIngredientData data, int amount)
    {
        itemData = data;//reference to the cheese ingredient data ScriptableObject, which contains all the info about this item (name, description, icon, etc.)
        quantity = amount;//amount is how many of this item (cheese ingredient) are in this slot
    }
    public bool IsFull()
    {
        return quantity >= itemData.maxStack;
    }
}