using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Aleksandra Rusek
 * 
 * Represents an enemy in the game.
 */
public class S_Enemy : MonoBehaviour
{
    [SerializeField] private GameObject projectileToSpawn; /* The projectile to be spawned by the enemy. */
    [SerializeField] private float fireRate = 0.3f; /* The rate at which the enemy fires projectiles. */
    [SerializeField] private Vector2 projectileSpawnOffSet = new Vector2(0f, -0.5f); /* The offset for spawning projectiles relative to the enemy's position. */
    [SerializeField] private Vector2 projectileSpeed = new Vector2(0f, -0.2f);  /* The speed at which projectiles travel. */
    [SerializeField] private bool move = true; /* Flag indicating whether the enemy should move. */
    [SerializeField] private int scoreToIncrease = 10; /* The score to increase when the enemy is defeated. */
    public bool isBoss = false; /* Flag indicating whether the enemy is a boss. */

    /**
    * Enumeration representing the types of enemies.
    */
    public enum EnemyType
    {
        Individual, Wave
    }
    public int ScoreToIncrease { get { return scoreToIncrease; } } /** Property to get the score to increase. */
   
    [SerializeField] public EnemyType enemyType; /* The type of the enemy. */
    private float minHeight = 0f, maxHeight = 0f; /* The minimum and maximum height for individual enemies. */
    private float minWidth = 0f, maxWidth = 0f; /* The minimum and maximum width for individual enemies. */
    float startHeight = 5.0f; /* The starting height for individual enemies. */
    private Vector2 minW, maxW; /* The minimum and maximum position for individual enemies to move between. */
    private Vector2 startH; /* The starting position for individual enemies. */
    private Vector2 vel1, vel2; /* Velocity vector for smooth movement. */
    [SerializeField] private  float smoothTime = 1f; /** The time it takes for the enemy to smoothly move between positions. */
    private float maxSpeed = 10f; /* The maximum speed for smooth movement. */
    private bool nearMin = false, nearMax = false, exec = false; /* Flags for tracking movement state. */


    /**
    * Spawns a projectile from the enemy.
    */
    private void Fire()
    {
        Vector2 targetPos = new Vector2(transform.position.x + projectileSpawnOffSet.x, transform.position.y + projectileSpawnOffSet.y);
        GameObject proj = Instantiate(projectileToSpawn, targetPos, projectileToSpawn.transform.rotation) as GameObject;
        if (proj.GetComponent<Rigidbody2D>())
            proj.GetComponent<Rigidbody2D>().AddForce(projectileSpeed, ForceMode2D.Impulse);
    }

    /**
     * Start is called at the start of the enemy's existence.
     * Initializes the movement parameters based on the enemy type.
     */
    void Start()
    {
        smoothTime = Random.Range(0f, 1f);
        if (projectileToSpawn != null)
            InvokeRepeating("Fire", 0.01f, fireRate);
        if (enemyType == EnemyType.Individual)
        {
            minHeight = Screen.height * 0.7f;
            maxHeight = Screen.height * 0.8f;
            Vector2 minH = Camera.main.ScreenToWorldPoint(new Vector2(0f, minHeight));
            Vector2 maxH = Camera.main.ScreenToWorldPoint(new Vector2(0f, maxHeight));
            float height = Random.Range(minH.y, maxH.y);        

            minWidth = Screen.width * 0.2f;
            maxWidth = Screen.width * 0.8f;
            minW = Camera.main.ScreenToWorldPoint(new Vector2(minWidth, 0f));
            maxW = Camera.main.ScreenToWorldPoint(new Vector2(maxWidth, 0f));
            minW.y = height;
            maxW.y = height;
            float width = Random.Range(minW.y, maxW.y);

            transform.position = new Vector2(width, startHeight);
        }
    }

    /**
     * Update is called once per frame.
     * Updates the movement of the individual enemy based on the SmoothDamp function.
     */
    void Update()
    {
        if(enemyType == EnemyType.Individual && move)
        {
            if(!exec)
            {
                transform.position = Vector2.SmoothDamp(transform.position, minW, ref vel1, smoothTime, maxSpeed, Time.deltaTime);
                if (transform.position.x - 0.1f <= minW.x)
                {
                    nearMin = true;
                    nearMax = false;
                    exec = true;
                }
            } else
            {
                if (transform.position.x - 0.1f <= minW.x)
                {
                    nearMin = true;
                    nearMax = false;
                }
                if (transform.position.x + 0.1f >= maxW.x)
                {
                    nearMin = false;
                    nearMax = true;
                }
                if (nearMax)
                    transform.position = Vector2.SmoothDamp(transform.position, minW, ref vel1, smoothTime, maxSpeed, Time.deltaTime);
                else if (nearMin)
                    transform.position = Vector2.SmoothDamp(transform.position, maxW, ref vel1, smoothTime, maxSpeed, Time.deltaTime);
            }
        }
    }
}
