using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Key;

/**
 * Author: Julia Bugaj
 * 
 * The Key class represents a key item in the game that can be collected by the player.
 * It implements the ICollectible interface and includes an event for key collection.
 */
public class Key : MonoBehaviour, ICollectible
{
    public static event HandleKeyCollected OnKeyCollected; /* Event triggered when a key is collected. */

    /**
     * Delegate for the key collection event.
     * 
     * @param itemData The ItemData associated with the collected key.
     */
    public delegate void HandleKeyCollected(ItemData itemData);
    public ItemData keyData; /* The data of the key item. */

    /**
     * Collects the key, triggers the collection event and destroys the key object with a delay.
     */
    public void Collect()
    {
        Debug.Log("Key collected");
        StartCoroutine(DestroyWithDelay());
        OnKeyCollected?.Invoke(keyData);
    }
    /**
     * Destroys the key object after a short delay.
     * 
     * @return IEnumerator for delaying the destruction.
     */
    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(0.2f); 
        Destroy(gameObject);
    }
}
