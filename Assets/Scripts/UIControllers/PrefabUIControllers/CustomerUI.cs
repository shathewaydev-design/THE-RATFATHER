using TMPro;
using UnityEngine;

public class CustomerUI : MonoBehaviour
{

    // Customer refs
    [Header("CustomerPrefab refs")]
    [SerializeField] private TextMeshProUGUI custName;
    [SerializeField] private TextMeshProUGUI custStatus;
    [SerializeField] private TextMeshProUGUI custPref;
    [SerializeField] private TextMeshProUGUI failedReqsNum;

    public void SetName(string name)
    {

        custName.text = name;

    }

    public void SetStatus(string status) 
    { 
           
        custStatus.text = status;   
    
    }

    public void SetPreference(string type, string quality)
    {

        custPref.text = "Prefers " + type + " - " + quality;

    }

    public void SetStrikes(string failedReqs)
    {

        failedReqsNum.text = failedReqs;

    }


}
