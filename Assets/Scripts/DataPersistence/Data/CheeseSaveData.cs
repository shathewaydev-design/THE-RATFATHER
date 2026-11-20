[System.Serializable]
public class CheeseSaveData
{
    //this is for saving cheese data
    public string itemID;
    public int quantity;

    public CheeseSaveData(string id, int amount)
    {
        itemID = id;
        quantity = amount;
    }
}