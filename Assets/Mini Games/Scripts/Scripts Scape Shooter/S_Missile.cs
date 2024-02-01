using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Aleksandra Rusek
 * 
 * Handles the behavior of a missile projectile in the game.
 */
public class S_Missile : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect; /* The explosion effect to be instantiated upon collision. */
    [SerializeField] private float explosionEffectLength = 10f; /* The duration of the explosion effect. */
    [SerializeField] private float timeToDestroy = 1f; /* The time it takes for the missile to self-destruct. */
    private bool collided = false; /* Flag indicating whether the missile has collided with an object. */
    [SerializeField] private int damage = 100; /* The amount of damage the missile inflicts. */
    private float timer = 0f; /* Timer to track the time since the missile was instantiated. */

    /**
     * Called when the missile collides with another 2D collider.
     * @param collision The collision data.
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collided = true;
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation) as GameObject;
            Destroy(explosion, explosionEffectLength);
        }
        if (collision.gameObject.GetComponent<S_HealthManager>())
            collision.gameObject.GetComponent<S_HealthManager>().DecreaseHealth(damage);
        S_PlayerController.Instance.ReleaseMissile(gameObject);
    }

    /**
     * Called once per frame to update the state of the missile.
     * Updates the timer and releases the missile if the specified time for self-destruction is reached and there is no collision.
     */
    void Update()
    {
            timer += Time.deltaTime;
            if ((timer >= timeToDestroy) && !collided)
            {
                timer = 0f;
                S_PlayerController.Instance.ReleaseMissile(gameObject);
            }
    }
}
