using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Aleksandra Rusek
 * 
 * Database class for managing items in a match-3 game.
 * This class provides functionality to access and manage the collection of items
 * available in the game. It loads the items from the "Match 3/Items/" resources folder
 * during initialization.
 */
public class M_ItemDatabase : MonoBehaviour
{
    /** Gets the array of items available in the game. */
    public static M_Item[] Items { get; private set; }

    /**
     * Initializes the item database.
     * This method is marked with the [RuntimeInitializeOnLoadMethod] attribute
     * to ensure that it is called before the scene is loaded. It loads all items
     * from the "Match 3/Items/" resources folder.
     */
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
    private static void Initialize() { Items = Resources.LoadAll<M_Item>("Match 3/Items/");}
}
