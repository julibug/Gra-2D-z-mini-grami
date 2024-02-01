using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Aleksandra Rusek
 * 
 * Represents a power-up in the game, which can be of different types (Projectile or Health).
 * Spawns at a random position and grants benefits to the player upon collision.
 */
public class S_PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType; /* Type of the power-up. */
    [SerializeField] private int numToGive = 10; /* Amount to give (missiles or health). */
    /**
     * Represents the types of power-ups that can be spawned in the game.
     * Projectile: Increases the number of projectiles or missiles available to the player.
     * Health: Restores the player's health.
     */
    public enum PowerUpType
    {
        Projectile, Health
    }

    /**
     * Start is called before the first frame update.
     * It sets the initial position of the power-up to a random location within the specified range.
     */
    private void Start()
    {
        transform.position = new Vector2(Random.Range(-7f, 7f), 4.4f);
    }

    /**
     * OnTriggerEnter2D is called when the Collider2D other enters the trigger.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<S_PlayerController>() != null) 
        {

            if (powerUpType == PowerUpType.Projectile)
            {
                if (S_GameStatsManager.Instance != null)
                    S_GameStatsManager.Instance.AddMissilesByAmount(numToGive);
            }
            else
            {
                if (collision.gameObject.GetComponent<S_HealthManager>())
                    collision.gameObject.GetComponent<S_HealthManager>().IncreaseHealth(numToGive);
            }
            Destroy(gameObject);
        }

    }
}
