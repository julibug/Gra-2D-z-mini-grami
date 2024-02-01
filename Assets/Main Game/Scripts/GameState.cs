using System;
using System.Collections.Generic;

/**
 * Author: Julia Bugaj
 * 
 * The GameState represents state of the game, including the player's inventory and the level name.
 */
[Serializable]
public class GameState
{
    public List<string> inventory = new List<string>(); /* List of item names in the player's inventory. */
    public string levelName; /* The name of the game level. */
}
