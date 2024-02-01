using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author: Aleksandra Rusek
 * 
 * MG_AudioManager class.
 * Manages audio functionalities such as playing and stopping music.
 */
public class MG_AudioManager : MonoBehaviour
{
    public static MG_AudioManager instance; /* Singleton instance of the audio manager. */
    public AudioSource musicSource; /* AudioSource for playing music. */
    [SerializeField] private float volumeMultiplier = 1f; /* Value to adjust manually volume level. */

    /**
     * Awake is called when the script instance is being loaded.
     * This method sets up the Singleton pattern for the MG_AudioManager, ensuring that only one instance exists.
     * The GameObject carrying this script is made persistent across scenes to maintain continuity.
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
     * Start is called before the first frame update.
     * This method is responsible for initializing the audio manager and retrieving the volume level.
     */
    private void Start()
    {
        GetVolume(); 
    }

    /**
     * Plays the music if the AudioSource is not null and not already playing.
     */
    public void PlayMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
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
     * Retrieves the volume level from PlayerPrefs and sets the AudioListener volume accordingly.
     */
    private void GetVolume()
    {
        float volumeLevel = PlayerPrefs.GetFloat("Volume", 1f);
        float newVolume = Mathf.Clamp01(volumeLevel * volumeMultiplier);
        AudioListener.volume = newVolume;
    }
}
