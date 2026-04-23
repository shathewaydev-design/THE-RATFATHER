using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private InventorySystem inventorySystem;

    public GameObject slotPrefab;
    public Transform slotParent;
    [SerializeField] private RecipeMenuUI recipeMenuUI;

    private void Awake()
    {
        inventorySystem = InventorySystem.Instance;
    }
    public void Refresh(List<InventorySlot> inventoryTest)
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        foreach (InventorySlot slot in inventoryTest)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotParent);
            UIInventorySlot uiSlot =
                slotObject.GetComponent<UIInventorySlot>();
            uiSlot.Initialize(recipeMenuUI);
            // button.Setup(inventorySlot.itemData);
            uiSlot.SetSlot(slot);
            
        }
    }
}
