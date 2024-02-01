using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The PlayOneShotBehaviour class extends StateMachineBehaviour and is responsible for playing a sound effect during state transitions. 
 */
public class PlayOneShotBehaviour : StateMachineBehaviour
{
    public AudioClip soundToPlay; /* AudioClip to be played. */
    public float volume = 1f; /* Volume level of the sound effect. */
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false; /* Options for when to play the sound: on enter, on exit or after a delay. */
    public float playDelay = 0.25f; /* Delay before playing the sound (if playAfterDelay is true). */
    private float timeSinceEntered; /* Variable to track time since entering the state. */
    private bool hasDelayedSoundPlayed = false; /* Variable to mark that the delayed sound has been played. */

    /** 
     * OnStateEnter is called when a transition starts and the state machine starts to evaluate this state.
     * 
     * @param animator The Animator component associated with the GameObject.
     * @param stateInfo The current state information.
     * @param layerIndex The layer index of the state machine.
     */
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
        }

        timeSinceEntered = 0f;
        hasDelayedSoundPlayed = false;
    }

    /**
     * OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks. 
     * 
     * @param animator The Animator component associated with the GameObject.
     * @param stateInfo The current state information.
     * @param layerIndex The layer index of the state machine.
     */
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playAfterDelay && !hasDelayedSoundPlayed)
        {
            timeSinceEntered += Time.deltaTime;

            if (timeSinceEntered > playDelay)
            {
                AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);

                hasDelayedSoundPlayed = true;
            }
        }
    }

    /** OnStateExit is called when a transition ends and the state machine finishes evaluating this state.
     * 
     * @param animator The Animator component associated with the GameObject.
     * @param stateInfo The current state information.
     * @param layerIndex The layer index of the state machine.
     */
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnExit)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
        }
    }
}
