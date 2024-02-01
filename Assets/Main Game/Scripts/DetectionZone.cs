using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The DetectionZone class is responsible for detecting and maintaining a list of colliders that enter its trigger zone.
 */
public class DetectionZone : MonoBehaviour
{
    Collider2D col;/* Reference to the Collider2D component attached to the game object. */
    public List <Collider2D> detectedColliders = new List<Collider2D>(); /* List of colliders currently detected within the trigger zone. */

    /**
     * Initializes the Collider2D component on Awake.
     */
    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    /**
     * Called when another collider enters the trigger zone.
     * Adds the collider to the list of detected colliders.
     * 
     * @param collision The Collider2D of the object entering the trigger zone.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColliders.Add(collision);
    }
    /**
     * Called when another collider exits the trigger zone.
     * Removes the collider from the list of detected colliders.
     * 
     * @param collision The Collider2D of the object exiting the trigger zone.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision);
    }
}
