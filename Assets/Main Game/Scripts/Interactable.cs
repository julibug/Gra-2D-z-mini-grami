using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Author: Julia Bugaj
 * 
 * The Interactable class is used with objects in the game that the player can interact with.
 */
public class Interactable : MonoBehaviour
{
    public bool isInRange;/* Indicates whether the player is in range to interact with the object. */
    public KeyCode interactKey; /* Key used to trigger the interaction. */
    public UnityEvent interactAction; /* Event invoked when the player interacts with the object. */
    public GameObject obj; /* Reference to the GameObject associated with the interactable object. */
    public AudioSource sound; /* Reference to the AudioSource component for playing interaction sounds. */

    /**
     * Initializes the sound variable on Awake.
     */
    private void Awake()
    {
        sound = GetComponent<AudioSource>();
    }
    /**
     * Update is called once per frame and checks for player interaction input.
     */
    void Update()
    {
        if(isInRange)
        {
            if (Input.GetKeyDown(interactKey))
            {
                if(sound)
                {
                    AudioSource.PlayClipAtPoint(sound.clip, gameObject.transform.position, 1f);
                }
                interactAction.Invoke();
            }
        }
    }
    /**
     * Called when another collider enters the trigger zone.
     * Sets isInRange to true and activates the associated GameObject.
     * 
     * @param collision The Collider2D of the object entering the trigger zone.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player is in range");
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
    /**
     * Called when another collider exits the trigger zone.
     * Sets isInRange to false and deactivates the associated GameObject.
     * 
     * @param collision The Collider2D of the object exiting the trigger zone.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player is not in range");
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
