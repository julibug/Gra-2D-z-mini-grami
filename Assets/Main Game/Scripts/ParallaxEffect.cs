using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The ParallaxEffect class creates a parallax effect based on the camera's movement and the distance to a follow target.
 */
public class ParallaxEffect : MonoBehaviour
{
    public Camera cam; /* Reference to the camera in the scene. */
    public Transform followTarget; /* The target to follow for parallax effect. */
    Vector2 startingPosition; /* The starting position of the object. */
    float startingZ; /* The starting z-position of the object. */

    /**
     * The movement of the camera since the start of the scene.
     */
    Vector2 camMoveSinceStart => (Vector2) cam.transform.position - startingPosition;
    /**
     * The distance between the object and the follow target in the z-axis.
     */
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
    /**
     * The clipping plane based on the camera's position and z-distance from the target.
     */
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    /**
     * The parallax factor calculated based on the z-distance from the target.
     */
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
    /**
     * Initializes the starting position and z-position.
     */
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.localPosition.z;
    }

    /**
     * Updates the position of the object based on the camera's movement and parallax factor.
     */
    void Update()
    {
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3 (newPosition.x, newPosition.y, startingZ);
    }
}
