using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Aleksandra Rusek
 * 
 * Represents a wave of enemies, managing the number and destruction of enemies within the wave.
 */
public class S_EnemyWave : MonoBehaviour
{
    internal int Length; /* Number of enemies in the wave. */

    /**
     * Gets the number of enemies in the wave.
     *
     * @return The number of enemies in the wave.
     */
    public int NumOfEnemies()
    {
        S_Enemy[] enemies = GetComponentsInChildren<S_Enemy>();
        return enemies.Length;

    }

    /**
    * Destroys the entire wave object.
    */
    public void DestroyWave()
    {
        Destroy(gameObject);
    }

}
