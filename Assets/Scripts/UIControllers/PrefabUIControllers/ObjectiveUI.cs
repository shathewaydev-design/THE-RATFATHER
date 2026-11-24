using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{

    // ObjectivePrefab refs
    [Header("ObjectivePrefab refs")]
    [SerializeField] private TextMeshProUGUI objType;
    [SerializeField] private TextMeshProUGUI objTitle;
    [SerializeField] private TextMeshProUGUI objStatus;
    [SerializeField] private TextMeshProUGUI objDesc;
    [SerializeField] private GameObject callButton;

    public void SetType(string type, Color color)
    {
        objType.text = type;
        objType.color = color;
    }

    public void SetTitle(string title)
    {
        objTitle.text = title;
    }

    public void SetStatus(string status)
    {
        objStatus.text = "Status: " + status;
    }

    public void SetDesc(string desc) 
    { 
        objDesc.text = desc;
    }

    public void SetCallButton(bool isCall)
    {

        if (isCall)
        {
            callButton.SetActive(true);
        } 
        else if (!isCall)
        {
            callButton.SetActive(false);
        }


    }


}
