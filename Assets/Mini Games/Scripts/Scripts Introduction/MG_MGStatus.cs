using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * Author: Aleksandra Rusek
 * 
 * MG_MGStatus class.
 * Manages and updates the status of mini-games.
 */
public class MG_MGStatus : MonoBehaviour
{
    public static MG_MGStatus Instance; /* Singleton instance of MG_MGStatus. */

    /**
     * Awake is called when the script instance is being loaded.
     * This method sets up the singleton instance of MG_MGStatus.
     */

    private void Awake()
    {
        Instance = this; 
    }

    /**
     * Start is called before the first frame update.
     * This method checks and sets the default difficulty if not already present in PlayerPrefs.
     */
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Difficulty"))
        {
            PlayerPrefs.SetFloat("Difficulty", 2f);
        }
    }

    /**
     * Marks a game as passed and saves the status.
     * @param name The name of the game to mark as passed.
     */
    public void GamePassed(string name)
    {
        PlayerPrefs.SetInt(name, 1);
    }
}
