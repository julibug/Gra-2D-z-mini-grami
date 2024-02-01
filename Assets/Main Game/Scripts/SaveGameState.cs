using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author: Julia Bugaj
 * 
 * The SaveGameState handles the saving and loading of the game state.
 */
public class SaveGameState : MonoBehaviour
{
    public GameObject notificationNoSave; /* Reference to the notification for no save file found. */
    public GameObject notificationLoading; /* Reference to the loading notification. */

    /**
     * Saves the current game state to a JSON file.
     * It includes the player's inventory and the current active scene.
     */
    public void SaveGame()
    {
        GameState state = new GameState();
        foreach (InventoryItem item in Inventory.instance.inventory)
        {
            state.inventory.Add(item.itemData.displayName);
        }
        state.levelName = SceneManager.GetActiveScene().name;
        string json = JsonUtility.ToJson(state);
        File.WriteAllText("game_state.json", json);
        Application.Quit();
    }

    /**
     * Loads the game state from a JSON file if it exists.
     * If the file is found, it restores the player's inventory and loads the saved scene.
     */
    public void LoadGame()
    {
        if (File.Exists("game_state.json"))
        {
            if (notificationLoading != null)
            {
                notificationLoading.SetActive(true);
                Invoke("DeactivateObject", 1.5f);
            }
            MG_AudioManager.instance.StopMusic();
            Destroy(MG_AudioManager.instance.gameObject);
            string json = File.ReadAllText("game_state.json");
            GameState state = JsonUtility.FromJson<GameState>(json);

            SceneManager.LoadSceneAsync(state.levelName);
            Inventory.instance.inventory.Clear();
            foreach (string displayName in state.inventory)
            {
                ItemData item = ScriptableObject.CreateInstance<ItemData>();
                item.displayName = displayName;
                    
                Inventory.instance.AddItem(item);
            }
            File.Delete("game_state.json");
        }
        else
        {
            if (notificationNoSave != null)
            {
                notificationNoSave.SetActive(true);
                Invoke("DeactivateObject", 1.5f);
            }
        }
    }
    /**
     * Deactivates the specified GameObject after a delay.
     */
    void DeactivateObject()
    {
        if (notificationNoSave != null)
        {
            notificationNoSave.SetActive(false);
        }
    }
}
