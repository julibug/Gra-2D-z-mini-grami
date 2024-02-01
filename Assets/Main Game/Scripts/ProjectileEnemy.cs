using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The ProjectileEnemy class represents a projectile fired by an enemy that can deal damage to objects with the Damageable component.
 * It moves in a specified direction, and upon colliding with a Damageable object, it deals damage and applies knockback.
 */
public class ProjectileEnemy : MonoBehaviour
{
    public int damage = 5; /* Amount of damage the projectile deals. */
    public Vector2 moveSpeed = new Vector2(2f, 0); /* Speed and direction of the projectile. */
    public Vector2 knockback = new Vector2(0, 0); /* Knockback force applied to the damaged object. */

    Rigidbody2D rb; /* Reference to the Rigidbody2D component. */

    /**
     * Awake is called when the script instance is being loaded.
     * It initializes the Rigidbody2D component.
     */
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    /**
     * Start is called before the first frame update.
     * It sets the initial velocity of the projectile based on its moveSpeed and direction.
     */
    void Start()
    {
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }
    /**
     * OnTriggerEnter2D is called when another collider enters the trigger zone.
     * It checks if the collided object has the Damageable component, then deals damage and applies knockback if successful.
     * If the object gets hit, the projectile is destroyed.
     *
     * @param collision The Collider2D of the object entering the trigger zone.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.localScale.x < 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            bool gotHit = damageable.Hit(damage, deliveredKnockback);
            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + damage);
                Destroy(gameObject);
            }
        }
    }
}
