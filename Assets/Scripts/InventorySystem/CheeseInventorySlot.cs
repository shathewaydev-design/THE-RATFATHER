[System.Serializable]
public class CheeseInventorySlot
{
    //store Final Cheese Data. you can reference this to grab items out and remove items from
    public FinalResultCheese finalCheeseData;
    public int quantity;

    public CheeseInventorySlot(FinalResultCheese data, int amount)
    {
        finalCheeseData = data;//reference to the final cheese data ScriptableObject, which contains all the info about this item (name, description, icon, etc.)
        quantity = amount;//amount is how many of this item (final cheese) are in this slot
    }
    public bool IsFull()
    {
        return quantity >= finalCheeseData.maxStack;
    }
}