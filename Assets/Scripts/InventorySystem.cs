using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

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

        Debug.Log(item.ingredientName + " added. Total: " + inventory[item]);
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
            Debug.LogWarning("Item not found in inventory: " + item.ingredientName);
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

        Debug.Log(foundKey.ingredientName + " removed.");

    }


    public int GetItemCount(CheeseIngredientData item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }
}