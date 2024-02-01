using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author: Julia Bugaj
 * 
 * The PlayerPositionManager class manages the saving and loading of the player's position in the game.
 */
public class PlayerPositionManager : MonoBehaviour
{
    private string playerPositionKey; /* PlayerPrefs key associated with the player's position in the current scene */

    /**
     * Start is called before the first frame update.
     * It sets the PlayerPrefs key based on the current scene and attempts to load the saved player position.
     * If a saved position exists, it moves the player to that position.
     */
    void Start()
    {
        playerPositionKey = SceneManager.GetActiveScene().name + "_PlayerPosition";

        if (PlayerPrefs.HasKey(playerPositionKey + "_x")  && PlayerPrefs.HasKey(playerPositionKey + "_y"))
        {
            float x = PlayerPrefs.GetFloat(playerPositionKey + "_x");
            float y = PlayerPrefs.GetFloat(playerPositionKey + "_y");

            Vector2 savedPosition = new Vector2(x, y);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = savedPosition;
        }
        else
        {
            Debug.Log("PlayerPrefs does not have player position");
        }
    }

    /**
     * Update is called once per frame.
     * It calls a function to save the player's current position in each frame.
     */
    void Update()
    {
        SavePlayerPosition();
    }
    /**
     * Saves the current position of the player in PlayerPrefs.
     */
    void SavePlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPosition = player.transform.position;

        PlayerPrefs.SetFloat(playerPositionKey + "_x", playerPosition.x);
        PlayerPrefs.SetFloat(playerPositionKey + "_y", playerPosition.y);

        PlayerPrefs.Save();
        
        Debug.Log("Player position was saved.");
        
    }
}
