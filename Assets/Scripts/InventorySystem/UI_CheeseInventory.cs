using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class UI_CheeseInventory : MonoBehaviour
{
    //This is a Cheese Container that spawns slot child based on inventory list
    private InventorySystem inventorySystem;

    public GameObject cheeseSlotPrefab;//Cheese Child
    public Transform cheeseSlotParent;//Cheese Container Parent
    //[SerializeField] private UseButton useButton;//save reference to pass to cheese button
    //[SerializeField] private GameObject useButton;//save reference to pass to cheese button

    private void Awake()
    {
        inventorySystem = InventorySystem.Instance;
    }
    public void RefreshCheeseUIInventory(List<CheeseInventorySlot> cheeseInventory)
    {
        foreach (Transform child in cheeseSlotParent)
        {
            Destroy(child.gameObject);
        }

        foreach (CheeseInventorySlot slot in cheeseInventory)//based on the order of each item inside the InventorySlot (place store all the data)
        {// add inventory slots on the UI when a new item is collected
            GameObject slotObject = Instantiate(cheeseSlotPrefab, cheeseSlotParent);
            // Debug.Log("Spawned Cheese Slot");
            UI_CheeseInventorySlot uiSlot =
                slotObject.GetComponent<UI_CheeseInventorySlot>();
            //uiSlot.Initialize(useButton);//initialize so player can press on the use button
            // button.Setup(inventorySlot.itemData);
            uiSlot.SetSlot(slot);
            
        }
    }
}
