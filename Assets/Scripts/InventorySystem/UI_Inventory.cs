using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    //This is a container that spawns slot child based on inventory list
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

        foreach (InventorySlot slot in inventoryTest)//based on the order of each item inside the InventorySlot (place store all the data)
        {//; add inventory slots on the UI when a new item is collected
            GameObject slotObject = Instantiate(slotPrefab, slotParent);
            UIInventorySlot uiSlot =
                slotObject.GetComponent<UIInventorySlot>();
            uiSlot.Initialize(recipeMenuUI);//initialize so player can press on the button and have it highlighted
            // button.Setup(inventorySlot.itemData);
            uiSlot.SetSlot(slot);
            
        }
    }
}
