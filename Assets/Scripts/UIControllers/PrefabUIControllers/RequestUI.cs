using TMPro;
using UnityEngine;

public class RequestUI : MonoBehaviour
{

    // RequestPrefab refs
    [Header("RequestPrefab refs")]
    public TextMeshProUGUI reqName;
    public TextMeshProUGUI reqType;
    public TextMeshProUGUI reqQuantity;
    public TextMeshProUGUI reqQuality;
    public TextMeshProUGUI estPayNum;

    public void SetName(string name)
    {
        reqName.text = name;
    }

    public void SetType(string type)
    {
        reqType.text = "Type: " + type;
    }

    public void SetQuantity(string quantity)
    {
        reqQuantity.text = "Quantity: " + quantity;
    }

    public void SetQuality(string quality)
    {
        reqQuality.text = "Minimum Quality: " + quality;
    }

    public void SetPayNum(string payNum)
    {
        estPayNum.text = payNum + " Chedz";
    }

    

}
