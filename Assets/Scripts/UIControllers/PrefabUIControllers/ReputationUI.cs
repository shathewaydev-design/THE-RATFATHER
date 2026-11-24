using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ReputationUI : MonoBehaviour
{

    // ReputationPrefab refs
    [Header("ReputationPrefab refs")]
    [SerializeField] private Slider repBar;
    [SerializeField] private TextMeshProUGUI repTitle;
    [SerializeField] private TextMeshProUGUI repMultiplier;

    public void SetRepBar(int repNum)
    {
        repBar.value = repNum;
    }

    public void SetTitle(string title)
    {
        repTitle.text = title;
    }

    public void SetMultiplier(float repNum)
    {
        repMultiplier.text = "" + repNum + "x";
    }


}
