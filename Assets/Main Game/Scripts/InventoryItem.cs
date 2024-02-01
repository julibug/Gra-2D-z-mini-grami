using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The InventoryItem class represents an item within the player's inventory.
 * It holds a reference to the associated ItemData.
 */
[Serializable]
public class InventoryItem
{
    public ItemData itemData; /* The data of the item in the inventory. */

    /**
     * Initializes the InventoryItem with the provided ItemData.
     * 
     * @param item The ItemData associated with the item.
     */
    public InventoryItem(ItemData item)
    {
        itemData = item;
    }
}
