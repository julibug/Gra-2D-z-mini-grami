using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author: Aleksandra Rusek
 * 
 * Represents an individual item in the match-3 game.
 * This ScriptableObject stores information about a specific item,
 * such as its value, sprite, and type.
 */
[CreateAssetMenu(menuName = "Match-3/Item")]
public class M_Item : ScriptableObject
{
    public int value; /* The value associated with the item. */
    public Sprite sprite; /* The sprite representing the item. */
    public int type; /* The type identifier for the item. */

}
