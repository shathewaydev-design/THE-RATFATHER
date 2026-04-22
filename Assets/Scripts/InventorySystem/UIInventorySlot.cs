using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;

    public void SetSlot(InventorySlot slot)
    {
        icon.sprite = slot.itemData.icon;

        quantityText.text = slot.quantity.ToString();
    }
}