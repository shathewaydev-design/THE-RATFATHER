using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    public List<InventorySlot> inventoryTest = new();//test new system; inventory
    [SerializeField] private UI_Inventory inventoryUI;
    private Dictionary<CheeseIngredientData, int> inventory = new Dictionary<CheeseIngredientData, int>();
    public IReadOnlyDictionary<CheeseIngredientData, int> Inventory => inventory; // so inventory can be read for quest requirements, etc.
    //this is like creating new definition in a dictionary, then put them in pages
    void Awake()
    {
        // Singleton pattern (simple version); avoid duplicates
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        RefreshUI();
    }

    public void AddItem(CheeseIngredientData item)
    {
        //ContainsKey() is a Dictionary method; 
        // this checks if the data from ScriptableObject is added to the dictionary, 
        //if yes, count up, i.e. 1 cowMilk ingredient (already in inventory) + 1 cowMilk = 2 cowMilk
        // if not adds data 
        if (inventory.ContainsKey(item))
        {
            inventory[item]++;
        }
        else
        {
            inventory.Add(item, 1);
        }

        //Debug.Log(item.ingredientName + " added. Total: " + inventory[item]);
    }

    public void RemoveItem(CheeseIngredientData item) // removing an item (soph added for quest completion)
    {

        CheeseIngredientData foundKey = null; // need to find by name, not by entire data reference

        foreach (var key in inventory.Keys)
        {
            if (key.ingredientName == item.ingredientName)
            {
                foundKey = key;
                break;
            }
        }

        if (foundKey == null)
        {
            //Debug.LogWarning("Item not found in inventory: " + item.ingredientName);
            return;
        }


        if (inventory.ContainsKey(foundKey) && inventory[foundKey] <= 1)
        {
            inventory.Remove(foundKey);
        }
        else
        {
            inventory[foundKey]--;

        }

        //Debug.Log(foundKey.ingredientName + " removed.");

    }


    public int GetItemCount(CheeseIngredientData item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }


/// Test new functions
    public void AddIngredientItem(CheeseIngredientData item, int amount = 1)
    {
        int remaining = amount;

        // fill existing stacks first
        foreach (InventorySlot slot in inventoryTest)
        {
            if (slot.itemData == item && !slot.IsFull())
            {
                int spaceLeft = item.maxStack - slot.quantity;

                int amountToAdd = Mathf.Min(spaceLeft, remaining);

                slot.quantity += amountToAdd;

                remaining -= amountToAdd;

                if (remaining <= 0)
                {
                    RefreshUI();
                    return;
                }
            }
        }

        // create new stacks if needed
        while (remaining > 0)
        {
            int amountToAdd = Mathf.Min(item.maxStack, remaining);

            InventorySlot newSlot =
                new InventorySlot(item, amountToAdd);

            inventoryTest.Add(newSlot);

            remaining -= amountToAdd;
        }

        RefreshUI();

        //Debug.Log($"Added {item.ingredientName}");
    }

    public void RemoveIngredientItem(CheeseIngredientData item, int amount = 1)
    {
        int remaining = amount;

        for (int i = inventoryTest.Count - 1; i >= 0; i--)
        {
            InventorySlot slot = inventoryTest[i];

            if (slot.itemData == item)
            {
                if (slot.quantity > remaining)
                {
                    slot.quantity -= remaining;
                    break;
                }
                else
                {
                    remaining -= slot.quantity;
                    inventoryTest.RemoveAt(i);
                }
            }

            if (remaining <= 0)
                break;
        }

        RefreshUI();
    }
    public void RemoveIngredientSlot(InventorySlot slot, int amount)
    {
        if (slot == null) return;

        if (!inventoryTest.Contains(slot)) return;

        if (slot.quantity > amount)
        {
            slot.quantity -= amount;
        }
        else
        {
            inventoryTest.Remove(slot);
        }

        RefreshUI();
    }
    private void RefreshUI()
    {
        inventoryUI.Refresh(inventoryTest);
    }
}