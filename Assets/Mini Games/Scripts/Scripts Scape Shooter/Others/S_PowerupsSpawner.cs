using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Aleksandra Rusek
 * 
 * Spawns power-ups at regular intervals with a specified probability.
 */
public class S_PowerupsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUps = new GameObject[1]; /* Array of power-up prefabs. */
    [SerializeField] [Range(0f, 1f)] private float probability = 0.5f; /* Probability of spawning a power-up. */
    [SerializeField] private float timeToSpawn = 6f; /* Time interval between power-up spawns. */
    private float timer = 0f; /* Timer to track the time elapsed since the last spawn. */

    /**
     * Update is called once per frame.
     * It checks if the time since the last spawn has reached the specified interval and, if so, spawns a power-up with a specified probability.
     */
    void Update()
    {
        int n = Random.Range(0, powerUps.Length);
        timer += Time.deltaTime;
        if (timer >= timeToSpawn)
        {
            if (powerUps[n] != null)
            {
                if (Random.value <= probability)
                {
                    Instantiate(powerUps[n].gameObject, transform);
                }
            }
            timer = 0f;
        }
    }

}
