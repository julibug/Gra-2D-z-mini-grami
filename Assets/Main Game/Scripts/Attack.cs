using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The Attack class is responsible for handling attacks.
 * It deals damage to damageable objects within its trigger area and applies knockback.
 */
public class Attack : MonoBehaviour
{
    public int attackDamage; /* The amount of damage the attack inflicts. */
    public Vector2 knockback = Vector2.zero; /* The knockback force applied to the target. */

    /**
     * OnTriggerEnter2D is called when another collider enters the trigger zone.
     * It checks if the collided object is damageable and applies damage and knockback if so.
     * 
     * @param collision The Collider2D of the object entering the trigger zone.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x < 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + attackDamage);
            }
        }
    }
}
