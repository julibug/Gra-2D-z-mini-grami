using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The Music class manages background music in the game.
 */
public class Music : MonoBehaviour
{
    public static Music instance; /* Static instance of the Music class to enable a singleton pattern. */
    public AudioSource musicSource; /* AudioSource for playing music. */

    /**
     * Initializes the singleton instance and ensures the object persists across scenes.
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
     * Stops the music if the AudioSource is not null and currently playing.
     */
    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    /**
     * Starts the music if the AudioSource is not null and not currently playing.
     */
    public void StartMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
}
