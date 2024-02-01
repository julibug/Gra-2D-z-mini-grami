using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author: Julia Bugaj
 * 
 * The SceneController class manages scene transitions and specific conditions for loading scenes.
 * It provides methods for loading scenes based on key requirements, starting or stopping music and handling special cases.
 */
public class SceneController : MonoBehaviour
{
    public static SceneController instance; /* Singleton instance of the SceneController. */
    private GameObject obj; /* Reference to a GameObject. */
    float delayInSeconds = 2.0f; /* Delay time for deactivating objects. */
    public AudioSource sound; /* AudioSource for sound effects. */
    public AudioSource soundForOpenEntrance; /* AudioSource for specific sound effects. */

    /**
     * Awake method sets up the singleton instance of SceneController.
     */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    /**
     * LoadSceneWithKey method loads a scene if the required key is present in the inventory.
     * It handles music control and displays a message if the key is missing.
     * @param sceneName The name of the scene to be loaded.
     * @param requiredKey The key required to access the scene.
     * @param obj The GameObject associated with the transition.
     * @param stopMusic Determines whether to stop the current music.
     * @param startMusic Determines whether to start playing music for the new scene.
     */
    public void LoadSceneWithKey(string sceneName, string requiredKey, GameObject obj, bool stopMusic, bool startMusic)
    {
        if (Inventory.instance.HasKey(requiredKey))
        {
            if (stopMusic)
            {
                Music.instance.StopMusic();
            }
            if (startMusic)
            {
                Music.instance.StartMusic();
            }
            SceneManager.LoadSceneAsync(sceneName);
            sound.Play();
        } else
        {
            Debug.Log("You need " + requiredKey + " to open the entrance");
            if (obj != null)
            {
                this.obj = obj;
                obj.SetActive(true);
                Invoke("DeactivateObject", delayInSeconds);
            }
        }
    }
    /**
     * DeactivateObject method deactivates the specified object after a delay.
     */
    void DeactivateObject()
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }

    /**
     * LoadSceneWithoutKey method loads a scene without key requirements.
     * It handles music control for the transition.
     * @param sceneName The name of the scene to be loaded.
     * @param stopMusic Determines whether to stop the current music.
     * @param startMusic Determines whether to start playing music for the new scene.
     */
    public void LoadSceneWithoutKey(string sceneName, bool stopMusic, bool startMusic)
    {
        if (stopMusic)
        {
            Music.instance.StopMusic();
        }
        if (startMusic)
        {
            Music.instance.StartMusic();
        }
        SceneManager.LoadSceneAsync(sceneName);
        soundForOpenEntrance.Play();
    }

    /**
     * LoadEndgame method loads the endgame scene if all specified keys are present in the inventory.
     * It handles music control and displays a message if any of the keys are missing.
     * @param sceneName The name of the endgame scene to be loaded.
     * @param requiredKey1 The first key required for the endgame scene.
     * @param requiredKey2 The second key required for the endgame scene.
     * @param requiredKey3 The third key required for the endgame scene.
     * @param obj The GameObject associated with the transition.
     * @param stopMusic Determines whether to stop the current music.
     * @param startMusic Determines whether to start playing music for the new scene.
     */
    public void LoadEndgame(string sceneName, string requiredKey1, string requiredKey2, string requiredKey3, GameObject obj, bool stopMusic, bool startMusic)
    {
        if (Inventory.instance.HasKey(requiredKey1) && Inventory.instance.HasKey(requiredKey2) && Inventory.instance.HasKey(requiredKey3))
        {
            if (stopMusic)
            {
                Music.instance.StopMusic();
            }
            if (startMusic)
            {
                Music.instance.StartMusic();
            }
            SceneManager.LoadSceneAsync(sceneName);
            Timer.instance.isTimerActive = false;
            float elapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0);
            float difficultyLevel = PlayerPrefs.GetFloat("Difficulty", 1f);
            MG_HighscoreManager.Instance.SaveHighscore(difficultyLevel, elapsedTime);
        }
        else
        {
            if (obj != null)
            {
                this.obj = obj;
                obj.SetActive(true);
                Invoke("DeactivateObject", delayInSeconds);
            }
        }
    }
    /**
     * LoadSceneWithMiniGame method loads a scene with a mini-game condition.
     * It checks PlayerPrefs to determine if the mini-game has been completed and handles music control accordingly.
     * @param sceneName The name of the main scene to be loaded.
     * @param sceneNameMiniGame The name of the mini-game scene to be loaded.
     * @param prefsName The name used for PlayerPrefs to check mini-game completion.
     * @param stopMusic Determines whether to stop the current music.
     * @param startMusic Determines whether to start playing music for the new scene.
     */
    public void LoadSceneWithMiniGame(string sceneName, string sceneNameMiniGame, string prefsName, bool stopMusic, bool startMusic)
    {
        if (!PlayerPrefs.HasKey(prefsName))
        {
            if (stopMusic)
            {
                Music.instance.StopMusic();
            }
            if (startMusic)
            {
                Music.instance.StartMusic();
            }

            SceneManager.LoadSceneAsync(sceneNameMiniGame);
        }
        else
        {
            if (sceneName == "LivingRoom") {
                Music.instance.StopMusic();
            }

            SceneManager.LoadSceneAsync(sceneName);
            soundForOpenEntrance.Play();
        }
    }

    /**
     * LoadLostMenu method loads the lost menu scene and stops the music.
     */
    public void LoadLostMenu()
    {
        Music.instance.StopMusic();
        if (File.Exists("game_state.json"))
        {
            File.Delete("game_state.json");
        }
        SceneManager.LoadSceneAsync("LostMenu");
    }

}
