using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private InventorySystem inventorySystem;
    private Transform itemSlotContainer;//parent
    private Transform itemSlotTemplate;//child
    public GameObject slotPrefab;
    public Transform slotParent;

    private void Awake()
    {
        inventorySystem = InventorySystem.Instance;
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }
    public void SetInventory(InventorySystem inventorySystem)
    {
        this.inventorySystem = inventorySystem;
        RefreshInventoryItems();
    }
    private void RefreshInventoryItems()
    {
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 100f;
        foreach (var item in inventorySystem.Inventory)
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            // Set the icon and amount text
            //var iconImage = itemSlot.Find("icon").GetComponent<UnityEngine.UI.Image>();
            //var amountText = itemSlot.Find("amount").GetComponent<UnityEngine.UI.Text>();
            //iconImage.sprite = item.Key.icon;
            //amountText.text = item.Value.ToString();

            x++;
            if (x >= 2) // 2 items per row
            {
                x = 0;
                y++;
            }
        }
    }
    public void Refresh(List<InventorySlot> inventoryTest)
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        foreach (InventorySlot slot in inventoryTest)
        {
            GameObject obj =
                Instantiate(slotPrefab, slotParent);

            UIInventorySlot uiSlot =
                obj.GetComponent<UIInventorySlot>();

            uiSlot.SetSlot(slot);
        }
    }
}
