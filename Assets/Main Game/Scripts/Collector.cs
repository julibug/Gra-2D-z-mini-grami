using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The Collector class is responsible for collecting objects that implement the ICollectible interface.
 * It triggers the collection and plays a sound when a valid collectible object enters its trigger zone.
 */
public class Collector : MonoBehaviour
{
    public AudioSource sound; /* Reference to the AudioSource for playing a collection sound. */
    public GameObject objectNotification; /* Reference to the notification object displayed upon collecting an item. */
    float delayInSeconds = 1.5f; /* Delay in seconds before disabling the notification object. */

    /**
     * OnTriggerEnter2D is called when another collider enters the trigger zone.
     * It checks if the collided object implements the ICollectible interface and collects it.
     * 
     * @param collision The Collider2D of the object entering the trigger zone.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible collectible = collision.GetComponent<ICollectible>();
        if (collectible != null )
        {
            collectible.Collect();
            sound.Play();
            if (objectNotification != null)
            {
                objectNotification.SetActive(true);
                StartCoroutine(DisableObjectAfterDelay(objectNotification, delayInSeconds));
            }
        }
    }
    /**
     * Coroutine to disable an object after a specified delay.
     * 
     * @param obj The GameObject to be disabled.
     * @param delay The delay in seconds before disabling the object.
     * @return IEnumerator for disable an object.
     */
    private IEnumerator DisableObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
