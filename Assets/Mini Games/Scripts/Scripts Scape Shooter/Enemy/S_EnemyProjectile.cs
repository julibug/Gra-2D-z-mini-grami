using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Aleksandra Rusek
 * 
 * Represents an enemy projectile that can cause damage to the player.
 */
public class S_EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected int damage = 10; /* The amount of damage the projectile can cause. */
    [SerializeField] protected float timeToDestroy = 1f; /* The time it takes for the projectile to destroy itself. */
    [SerializeField] protected bool destroyProjectile = true; /* Flag indicating whether the projectile should destroy itself. */
    private bool collided = false; /* Flag indicating whether the projectile has collided with another object. */
    private float timer = 0f; /* Timer to track the projectile's existence. */


    /**
     * Update is called once per frame.
     * Checks if the projectile should destroy itself based on the destroyProjectile flag and timeToDestroy.
     * Destroys the projectile if the conditions are met.
     */
    void Update()
    {
        if (!destroyProjectile)
            return;
        timer += Time.deltaTime;
        if ((timer >= timeToDestroy) && !collided)
        {
            timer = 0f;
            Destroy(gameObject);
        }
    }

    /**
    * Called when the projectile collides with another object.
    * If the collision involves an object with S_HealthManager, decreases the health of that object and destroys the projectile.
    * @param collision The Collision2D data associated with this collision.
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collided = true;
        if (collision.gameObject.GetComponent<S_HealthManager>())
            collision.gameObject.GetComponent<S_HealthManager>().DecreaseHealth(damage);
        Destroy(gameObject);
    }
}
