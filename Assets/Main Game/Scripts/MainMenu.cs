using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/**
 * Author: Julia Bugaj
 * 
 * The MainMenu class handles menu functionality, such as starting the game, quitting, and navigating back to the main menu.
 */
public class MainMenu : MonoBehaviour
{
    
    public GameObject menu; /* Reference to the menu GameObject.*/
    public bool frozen = false; /* Indicating whether the game is frozen. */

    /**
     * Method to start the game, called when the button is clicked.
     * Deletes all PlayerPrefs, finds and destroys objects with the tag "DontDestroyOnLoad", deletes the "game_state.json" file if it exists and loads the "MG_Options" scene.
     */
    public void PlayGame()
    {
        PlayerPrefs.DeleteAll();

        GameObject[] dontDestroyObjects = GameObject.FindGameObjectsWithTag("DontDestroyOnLoad");
        foreach (GameObject obj in dontDestroyObjects)
        {
            Destroy(obj);
        }
        if (File.Exists("game_state.json"))
        {
            File.Delete("game_state.json");
        }
            SceneManager.LoadSceneAsync("MG_Options");
    }

    /**
     * Method to quit the game, called when the button is clicked.
     * Deletes the "game_state.json" file if it exists.
     */
    public void QuitGame()
    {
        if (File.Exists("game_state.json"))
        {
            File.Delete("game_state.json");
        }
        Application.Quit();
    }

    /**
     * Method to go back to the main menu, called when the button is clicked.
     * Deletes the "game_state.json" file if it exists.
     */
    public void GoBackToMenu()
    {
        Music.instance.StopMusic();
        Destroy(Music.instance.gameObject);
        if (File.Exists("game_state.json"))
        {
            File.Delete("game_state.json");
        }
        Timer.instance.isTimerActive = false;
        SceneManager.LoadSceneAsync("MainMenu");
        if (frozen)
        {
            ResumeGame();
        }
    }

    /**
     * Method is called when the escape key is pressed.
     * Checks if the escape key is pressed and if the menu GameObject is not null, sets the menu GameObject to active and froze the game.
     */
    public void OnEsc(InputAction.CallbackContext context)
    {
        if (context.started && menu != null) {
            Time.timeScale = 0.0f;
            menu.SetActive(true);
            frozen = true;
        }
    }
    /**
     * Resumes the game.
     * This method sets the game time scale to normal (1.0f) and indicates that the game is no longer frozen.
     */
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        frozen = false;
    }
}
