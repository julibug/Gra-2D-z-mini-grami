using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Author: Aleksandra Rusek
 * 
 * SC_ClickPoint class.
 * Handles the behavior of clickable points in the dot-connecting game.
 */
public class SC_ClickPoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; /* SpriteRenderer component for changing the color of the point. */
    private Color originalColor; /* Original color of the point. */
    public Color clickedColor = Color.blue; /* Color to change to when the point is clicked. */
    public GameObject clickEffectPrefab; /* Prefab for the click effect. */
    private GameObject currentClickEffect; /* Reference to the current click effect. */
    private GameObject clickEffect; /* Reference to the click effect. */

    /**
     * Start is called before the first frame update.
     * Initializes the SpriteRenderer component and stores the original color of the point.
     */
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    /**
     * Changes the color of the point and adds a click effect.
     * @param point The point to change the color.
     */
    public void ChangePointColor(Transform point)
    {
        if (spriteRenderer != null)
        {
            if (clickEffectPrefab != null)
            {
                clickEffect = Instantiate(clickEffectPrefab, point.position, Quaternion.identity);
                SC_ClickEffectDestroyer destroyer = clickEffect.AddComponent<SC_ClickEffectDestroyer>();
                destroyer.Initialize(point.gameObject);
            }
        }
    }

    /**
     * Deactivates the click effect.
     */
    public void DeactivateClickEffect()
    {
        if (clickEffect != null)
        {
            clickEffect.SetActive(false);
        }
    }
}
