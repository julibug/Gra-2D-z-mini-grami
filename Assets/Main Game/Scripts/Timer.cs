using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The Timer class manages a countdown timer in the game.
 * It tracks the elapsed time and triggers a specified action when the target time is reached.
 */
public class Timer : MonoBehaviour
{
    private float startTime; /* The time when the timer starts. */
    public bool isTimerActive = false; /* Indicating whether the timer is active. */
    public float elapsedTime; /* The elapsed time since the timer started. */
    public float targetTime = 1800f; /* Target time in seconds (30 minutes). */
    public static Timer instance; /* Singleton instance of the Timer. */

    /**
     * Awake method sets up the singleton instance of Timer.
     */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /**
     * Start method initializes the timer by recording the start time.
     */
    void Start()
    {
        float elapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0);
        if (elapsedTime > 0)
        {
            startTime = Time.time - elapsedTime;
        }
        else
        {
            startTime = Time.time;
        }
        isTimerActive = true;
    }
    /**
     * Update method is called once per frame and manages the countdown logic.
     * It calculates the elapsed time and checks if the target time has been reached.
     */
    void Update()
    {
        if (isTimerActive)
        {
            elapsedTime = Time.time - startTime;

            if (elapsedTime >= targetTime)
            {
                isTimerActive = false;

                HandleTimeUp();
            }
            PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
        }
    }
    /**
     * HandleTimeUp method is triggered when the target time is reached.
     * It loads the "LostMenu" scene using the SceneController.
     */
    private void HandleTimeUp()
    {
        Debug.Log("Game over");
        SceneController.instance.LoadLostMenu();
    }

}
