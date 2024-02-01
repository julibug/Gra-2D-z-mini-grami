using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Aleksandra Rusek
 * 
 * Manages the spawning of enemies in the game.
 */
public class S_EnemySpawner : MonoBehaviour
{
    [SerializeField] private int maxEnemies = 3; /* Maximum number of enemies to be spawned. */
    [SerializeField] private GameObject[] enemiesToSpawn = new GameObject[2]; /* Array of enemy prefabs to spawn.*/
    [SerializeField] private int interval = 10; /* Interval for spawning enemies. */
    [SerializeField] private int enemyDeathsToSpawnBoss = 4; /* Number of enemy deaths required to spawn a boss. */
    [SerializeField] private GameObject enemyBoss; /* Boss enemy prefab. */
    public static S_EnemySpawner Instance; /* Singleton instance of the enemy spawner. */
    public static int enemiesDefeated = 0; /* Number of enemies defeated. */
    private bool exec = false; /* Flag for tracking the execution of spawning a boss enemy. */


    /**
    * Check if a boss enemy has already been spawned.
    * @return True if a boss enemy is already spawned, false otherwise.
    */
    private bool IsBossAlreadySpawned()
    {
        S_Enemy[] temp = GameObject.FindObjectsOfType<S_Enemy>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].isBoss)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Get the total number of enemies in the scene.
     * @return The total number of enemies.
     */
    private int GetNumberOfEnemies()
    {
        int num = 0;
        S_Enemy[] temp = GameObject.FindObjectsOfType<S_Enemy>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].enemyType == S_Enemy.EnemyType.Individual)
                num++;
            if (temp[i].enemyType == S_Enemy.EnemyType.Wave)
                num += GameObject.FindObjectOfType<S_EnemyWave>().NumOfEnemies();
        }
        return num;
    }

    /**
     * Spawn random enemies.
     */
    private void SpawnEnemies()
    {
        
        if (GetNumberOfEnemies() >= maxEnemies)
            return;
        int n = Random.Range(0, enemiesToSpawn.Length);
        Instantiate(enemiesToSpawn[n]);
    }

    /**
     * Start is called before the first frame update.
     * Spawns initial enemies when the scene starts.
     */
    private void Start()
    {
        SpawnEnemies();
    }

    /**
     * Spawn a boss enemy.
     */
    private void SpawnEnemyBoss()
    {
        if (!IsBossAlreadySpawned() && enemyBoss != null)
            Instantiate(enemyBoss);
    }

    /**
     * Set the execution flag for spawning a boss enemy.
     * @param type The value to set the execution flag.
     */
    public void SetExec(bool type)
    {
        exec = type;
    }

    /**
     * Update is called once per frame.
     * Manages the spawning of enemies based on the number of defeated enemies and time intervals.
     */
    private void Update()
    {
        if (enemiesDefeated >= enemyDeathsToSpawnBoss)
        {
            if (!exec)
            {
                SpawnEnemyBoss();
                exec = true;
                enemiesDefeated = 0;
               
                exec = false;
            }
        }
        else
        {
            if (Time.frameCount % interval == 0)
                SpawnEnemies();
        }
    }
}
