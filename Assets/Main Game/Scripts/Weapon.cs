using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Weapon;

/**
 * Author: Julia Bugaj
 * 
 * The Weapon class represents a collectible weapon in the game.
 */
public class Weapon : MonoBehaviour, ICollectible
{
    public static event HandleWeaponCollected OnWeaponCollected; /* Event triggered when a weapon is collected. */

    /**
     * Delegate for the weapon collection event.
     * 
     * @param itemData The ItemData associated with the collected weapon.
     */
    public delegate void HandleWeaponCollected(ItemData itemData);
    public ItemData weaponData; /* Data associated with the weapon. */

    /**
     * Collects the weapon, triggers the collection event and destroys the GameObject with a slight delay.
     */
    public void Collect()
    {
        Debug.Log("Weapon collected");
        StartCoroutine(DestroyWithDelay());
        OnWeaponCollected?.Invoke(weaponData);
    }
    /**
     * Destroys the GameObject with a delay after being collected.
     * 
     * @return IEnumerator for delaying the destruction.
     */
    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(0.2f); 
        Destroy(gameObject);
    }
}
