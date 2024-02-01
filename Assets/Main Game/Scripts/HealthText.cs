using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The HealthText class handles the animation and fading of health-related text displayed in the game.
 * It allows the text to move upwards and gradually fade out over time.
 */
public class HealthText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0,65,0); /* Speed at which the text moves in the vertical direction. */
    public float timeToFade = 1f; /* Time it takes for the text to fade out completely. */

    RectTransform textTransform; /* Reference to the RectTransform component of the text. */
    TextMeshProUGUI textMeshPro; /* Reference to the TextMeshProUGUI component for displaying text. */

    private float timeElapsed = 0f;/* Elapsed time since the text was instantiated. */
    private Color startColor;/* Starting color of the text. */

    /**
     * Initializes the required components on Awake and stores the starting color.
     */
    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }
    /**
     * Updates the position, alpha (transparency), and destroys the text object when fading is complete.
     */
    private void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;
        timeElapsed += Time.deltaTime;

        if(timeElapsed < timeToFade)
        {
            float fadeAlpha = startColor.a * (1 - timeElapsed / timeToFade);
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
