using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Julia Bugaj
 * 
 * The HealthBar class manages the UI representation of the player's health using a slider and text.
 * It subscribes to the healthChange event of the player's Damageable component and updates the UI accordingly.
 */
public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;/* Reference to the Slider UI element representing the health bar. */
    public TMP_Text healthBarText;/* Reference to the TextMeshPro UI element displaying health information. */
    Damageable playerDamageable;/* Reference to the Damageable component of the player. */

    /**
     * Initializes the playerDamageable reference on Awake.
     */
    private void Awake()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("No player founded");
        }
        playerDamageable = player.GetComponent<Damageable>();
    }
    /**
    * Initializes the health bar and text on Start.
    */
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);
        healthBarText.text = playerDamageable.Health + "/" + playerDamageable.MaxHealth;
    }
    /**
     * Subscribes to the player's healthChange event on enabling the HealthBar.
     */
    private void OnEnable()
    {
        playerDamageable.healthChange.AddListener(OnPlayerHealthChanged);
    }

    /**
     * Unsubscribes from the player's healthChange event on disabling the HealthBar.
     */
    private void OnDisable()
    {
        playerDamageable.healthChange.RemoveListener(OnPlayerHealthChanged);
    }
    /**
     * Calculates the percentage of health for updating the slider value.
     * 
     * @param currentHealth Current health value.
     * @param maxHealth Maximum health value.
     * @return The percentage of currentHealth relative to maxHealth.
     */
    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }
    /**
     * Handles the player's health change event by updating the slider and text.
     * 
     * @param newHealth The new health value.
     * @param maxHealth The maximum health value.
     */
    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = newHealth + "/" + maxHealth;
    }
}
