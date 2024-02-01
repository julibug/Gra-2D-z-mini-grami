using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Aleksandra Rusek
 * 
 * Manages game statistics, such as the number of lasers and missiles available.
 */
public class S_GameStatsManager : MonoBehaviour
{
    public static S_GameStatsManager Instance; /* Singleton instance of S_GameStatsManager. */
    [SerializeField] private int numOfLasers = 100, numOfMissiles = 100; /* Basic number of missiles and lasers. */
    [SerializeField] private Text lasersText, missilesText; /*  UI text objects displaying the number of lasers and missiles. */
    [SerializeField] private bool infiniteMissiles = false, infiniteLasers = false; /* Flags indicating whether there are infinite missiles or lasers. */
    private string MISSILES_KEY = "NumOfMissiles"; /* Key to save number of missiles. */

    [Header("Audio")]
    [SerializeField] private AudioClip laserSound; /* Sound played when shooting lasers. */
    [SerializeField] private AudioClip missileSound; /* Sound played when shooting missiles. */
    [SerializeField] private AudioClip backgroundMusic; /* Background music. */
    private AudioSource audioSource; /* Audio source component for playing sounds and music. */
    [SerializeField] float volumeSound = 0.1f; /* Volume sound level. */

    /**
     * Checks if shooting the specified amount of lasers is possible.
     * @param amount The amount of lasers to shoot.
     * @return True if shooting is possible, false otherwise.
     */
    public bool CheckIfCanShootLaser(int amount)
    {
        if (infiniteLasers)
            return true;
        bool p = false;
        if (numOfLasers - amount >= 0)
            p = true;
        return p;
    }

    /**
     * Checks if shooting the specified amount of missiles is possible.
     * @param amount The amount of missiles to shoot.
     * @return True if shooting is possible, false otherwise.
     */
    public bool CheckIfCanShootMissiles(int amount)
    {
        if (infiniteMissiles)
            return true;
        bool p = false;
        if (numOfMissiles - amount >= 0)
            p = true;
        return p;
    }

    /**
     * Shoots the specified amount of lasers.
     * @param amount The amount of lasers to shoot.
     */
    public void ShootLasersByAmount(int amount)
    {
        if (infiniteLasers)
        {
            PlaySound(laserSound);
            return;
        }
        if (numOfLasers - amount >= 0)
        {
            numOfLasers -= amount;
            PlaySound(laserSound);
        }
            
    }

    /**
    * Shoots the specified amount of missiles.
    * @param amount The amount of missiles to shoot.
    */
    public void ShootMissilesByAmount(int amount)
    {
        if (infiniteMissiles)
        {
            PlaySound(missileSound);
            return;
        }
        if (numOfMissiles - amount >= 0)
        {
            numOfMissiles -= amount;
            PlaySound(missileSound);
        }
    }

    /**
     * Adds the specified amount of missiles.
     * @param amount The amount of missiles to add.
     */
    public void AddMissilesByAmount(int amount)
    {
        if (infiniteMissiles)
            return;
        if (numOfMissiles >= 0)
            numOfMissiles += amount;
    }

    /**
     * Plays the specified sound.
     * @param clip The audio clip to be played.
     */
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    /**
     * Start is called before the first frame update.
     * Initializes the singleton instance of S_GameStatsManager, sets up the audio source component for playing sounds and music,
     * and plays the background music if available.
     */
    void Start()
    {
        Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volumeSound;
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
        if (PlayerPrefs.HasKey(MISSILES_KEY))
        {
            numOfMissiles = PlayerPrefs.GetInt(MISSILES_KEY);
        }
        else
        {
            numOfMissiles = 10;
        }
    }


    /**
     * Update is called once per frame.
     * Updates the UI text objects displaying the number of lasers and missiles.
     */
    void Update()
    {
        if (lasersText != null)
            lasersText.text = "Lasery: " + numOfLasers.ToString();
        if (missilesText != null)
            missilesText.text = "Torpedy: " + numOfMissiles.ToString();
        if(Input.GetKey(KeyCode.Escape))
        {
            PlayerPrefs.SetInt(MISSILES_KEY, numOfMissiles);
        }
    }

    /**
     * Delete missiles key from player prefs.
     */

    public void deleteKeyMissiles()
    {
        PlayerPrefs.DeleteKey(MISSILES_KEY);
        PlayerPrefs.Save();
        numOfMissiles = 10;
    }
}
