using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Aleksandra Rusek
 * 
 * Manages the health of an object and handles health-related events.
 */
public class S_HealthManager : MonoBehaviour
{
    private int currentHealth; /* The current health of the object. */
    [SerializeField] private int minHealth = 0, maxHealth = 100; /* The minimum and maximum health values. */
    [SerializeField] private Image healthFill; /* UI image representing the health fill. */
    [SerializeField] private Text healthText; /* UI text displaying the current health. */
    [SerializeField] private GameObject explosionEffectPrefab; /* Prefab of the explosion effect upon death. */
    [SerializeField] private AudioClip deathSound; /* Sound played upon death. */
    private AudioSource audioSource; /* Audio source component for playing sounds. */
    private bool isDead = false;  /* Flag indicating whether the object is dead. */
    private string HEALTH_KEY = "HealthKey"; /* Name of player prefs. */

    /**
     * Increases the health of the object.
     * @param amount The amount by which to increase the health.
     */
    public void IncreaseHealth(int amount = 1)
    {
        if (currentHealth < maxHealth)
            currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, minHealth, maxHealth);
    }

    /**
     * Decreases the health of the object.
     * @param amount The amount by which to decrease the health.
     */
    public void DecreaseHealth(int amount = 1)
    {
        if (currentHealth < minHealth)
            Kill();
        if (currentHealth > minHealth)
            currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, minHealth, maxHealth);
    }

    /**
     * Initiates the explosion effect and plays the death sound.
     */
    private void Explode()
    {
        audioSource.PlayOneShot(deathSound);
        if (explosionEffectPrefab == null)
            return;

        GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
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
     * Handles the death of the object.
     * Initiates the explosion, updates scores, and triggers game over if the player dies.
     */
    private void Kill()
    {
        if (isDead) return; 
        isDead = true; 
        Explode();
        if (GetComponent<S_Enemy>())
        {
            S_ScoreManager.Instance.IncreaseScore(GetComponent<S_Enemy>().ScoreToIncrease);
            S_EnemySpawner.enemiesDefeated++;
        }
        else if (GetComponent<S_PlayerController>())
        {
            if (S_ScoreManager.Instance != null)
                S_ScoreManager.Instance.SetHighScore();
            GameObject.FindFirstObjectByType<S_GameOverManager>().GameOver();
        }
        Destroy(gameObject);
    }


    /**
     * Start is called before the first frame update.
     * Initializes the current health to the maximum health, sets up the AudioSource component for playing sounds,
     * and adjusts the volume of the audio source.
     */
    void Start()
    {
        if (GetComponent<S_PlayerController>() != null)
        {
            if (PlayerPrefs.HasKey(HEALTH_KEY))
            {
                currentHealth = PlayerPrefs.GetInt(HEALTH_KEY);
            }
            else
            {
                currentHealth = maxHealth;
            }
        }
        else
        {
            currentHealth = maxHealth; 
        }
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.1f;
    }

    /**
     * Calculates the fill amount for the health bar.
     * @param cur The current health value.
     * @param max The maximum health value.
     * @return The fill amount as a float.
     */
    private float FillAmount(int cur, int max)
    {
        return (float)((float)cur / (float)max);
    }

    /**
     * Update is called once per frame.
     * Updates the UI text displaying the current health, fills the health bar, and triggers death events if health reaches the minimum value.
     */
    void Update()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
        if (healthFill != null)
        {
            healthFill.fillAmount = FillAmount(currentHealth, maxHealth);
        }
        if (currentHealth <= minHealth)
        {
            PlaySound(deathSound);
            Invoke("Kill", 0.1f);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            if (GetComponent<S_PlayerController>() != null)
            {
                PlayerPrefs.SetInt(HEALTH_KEY, currentHealth);
            }
        }
    }

    /**
     * Delete player health key.
     */
    public void deleteKeyHealth()
    {
        PlayerPrefs.DeleteKey(HEALTH_KEY);
    }
}
