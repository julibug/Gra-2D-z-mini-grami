using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The ItemData class is a ScriptableObject that represents the data for an in-game item.
 * It includes properties such as the display name and icon of the item.
 */

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string displayName; /* The display name of the item. */
    public Sprite icon; /* The icon representing the item. */
}
