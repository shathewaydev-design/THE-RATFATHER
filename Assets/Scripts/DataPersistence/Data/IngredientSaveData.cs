[System.Serializable]
public class IngredientSaveData
{
    //this is for saving ingredient data
    public string itemID;
    public int quantity;

    public IngredientSaveData(string id, int amount)
    {
        itemID = id;
        quantity = amount;
    }
}