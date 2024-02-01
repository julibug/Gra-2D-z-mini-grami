using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The Inventory class manages the player's inventory, including the collection and querying of items.
 */
public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>(); /* List of InventoryItem objects representing the items in the inventory. */

    public static Inventory instance; /* Static instance of the Inventory class to enable a singleton pattern. */

    /**
     * Initializes the singleton instance and ensures the object persists across scenes.
     */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /**
     * Subscribes to events for adding keys and weapons to the inventory on enabling the Inventory.
     */
    private void OnEnable()
    {
        Key.OnKeyCollected += AddItem;
        Weapon.OnWeaponCollected += AddItem;
    }
    /**
     * Unsubscribes from events on disabling the Inventory.
     */
    private void OnDisable()
    {
        Key.OnKeyCollected -= AddItem;
        Weapon.OnWeaponCollected -= AddItem;
    }
    /**
     * Adds an item to the inventory based on the provided ItemData.
     * 
     * @param itemData The data of the item to be added.
     */
    public void AddItem(ItemData itemData)
    {
        InventoryItem newItem = new InventoryItem(itemData);
        inventory.Add(newItem);
        Debug.Log($"Added {itemData.displayName} to the inventory.");
    }
    /**
     * Checks if the inventory contains a key with the specified name.
     * 
     * @param keyName The name of the key to check.
     * @return True if the key is in the inventory, false otherwise.
     */
    public bool HasKey(string keyName)
    {
        foreach (InventoryItem item in inventory)
        {
            if (item.itemData.displayName == keyName)
            {
                Debug.Log(keyName + " is in inventory");
                return true; 
            }
        }
        Debug.Log(keyName + " is not in inventory");
        return false; 
    }
    /**
     * Checks if the inventory contains a bow.
     * 
     * @return True if the bow is in the inventory, false otherwise.
     */
    public bool HasBow()
    {
        foreach (InventoryItem item in inventory)
        {
            if (item.itemData.displayName == "Bow")
            {
                Debug.Log("Bow is in inventory");
                return true; 
            }
        }
        Debug.Log("Bow is not in inventory");
        return false;
    }
    /**
     * Checks if the inventory contains an ax.
     * 
     * @return True if the ax is in the inventory, false otherwise.
     */
    public bool HasAx()
    {
        foreach (InventoryItem item in inventory)
        {
            if (item.itemData.displayName == "Ax")
            {
                Debug.Log("Ax is in inventory");
                return true; 
            }
        }
        Debug.Log("Ax is not in inventory");
        return false;
    }
}
