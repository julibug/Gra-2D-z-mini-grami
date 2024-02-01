using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The ProjectileLauncher class is responsible for firing projectiles from a specified launch point.
 * It instantiates a projectile prefab and adjusts its scale based on the orientation of the launcher.
 */
public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint; /* The position where the projectile is launched from. */
    public GameObject projectilePrefab; /* Prefab of the projectile to be launched. */

    /**
     * FireProjectile method is called to launch a projectile.
     * It instantiates a projectile at the launch point and adjusts its scale based on the orientation of the launcher.
     */
    public void FireProjectile()
    {
            GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
            Vector3 originalScale = projectile.transform.localScale;
            projectile.transform.localScale = new Vector3(
                originalScale.x * (transform.localScale.x > 0 ? 1 : -1),
                originalScale.y,
                originalScale.z);
    }
}
