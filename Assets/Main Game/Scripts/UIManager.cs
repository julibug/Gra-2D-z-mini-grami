using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

/**
 * Author: Julia Bugaj
 * 
 * The UIManager class handles UI-related functionalities, such as displaying damage and health text on the canvas.
 */
public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab; /* Prefab for damage text. */
    public GameObject healthTextPrefab; /* Prefab for health text. */
    public Canvas gameCanvas; /* Reference to the game canvas */
    /**
     * Method called when the script instance is being loaded.
     * It initializes to find or create the game canvas.
     */
    private void Awake()
    {
        FindOrCreateCanvas();
    }
    /**
     * Method called when the object becomes enabled and active.
     * It subscribes to character damage and heal events.
     */
    private void OnEnable()
    {
        CharacterActions.characterDamaged += CharacterTookDamage;
        CharacterActions.characterHealed += CharacterHealed;
    }
    /**
     * Method called when the object becomes disabled or inactive.
     * It unsubscribes from character damage and heal events.
     */
    private void OnDisable()
    {
        CharacterActions.characterDamaged -= CharacterTookDamage;
        CharacterActions.characterHealed -= CharacterHealed;
    }
    /**
     * Displays damage text on the canvas when a character takes damage.
     * @param character The character GameObject.
     * @param damageReceived The amount of damage received.
     */
    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        FindOrCreateCanvas();
        Vector3 spawnPoint = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPoint, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = damageReceived.ToString();
    }
    /**
     * Displays health text on the canvas when a character is healed.
     * @param character The character GameObject.
     * @param healthRestored The amount of health restored.
     */
    public void CharacterHealed(GameObject character, int healthRestored)
    {
        FindOrCreateCanvas();
        Vector3 spawnPoint = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPoint, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = healthRestored.ToString();
    }
    /**
     * Finds or creates the game canvas.
     * If the canvas is not already set, it attempts to find it in the scene. If not found, it creates a new canvas GameObject.
     */
    private void FindOrCreateCanvas()
    {
        if (gameCanvas == null)
        {
            gameCanvas = FindObjectOfType<Canvas>();

            if (gameCanvas == null)
            {
                gameCanvas = new GameObject("GameCanvas").AddComponent<Canvas>();
            }
        }
    }
}
