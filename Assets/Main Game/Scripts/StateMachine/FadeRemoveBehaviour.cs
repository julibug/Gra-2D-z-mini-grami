using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * Author: Julia Bugaj
 * 
 * The FadeRemoveBehaviour class extends StateMachineBehaviour and is responsible for fading out a sprite during a state transition.
 */
public class FadeRemoveBehaviour : StateMachineBehaviour
{
    public float fadeTime = 1.1f; /* Time it takes for the sprite to fade out. */
    private float timeElapsed = 0f; /* Variable to track the elapsed time during the fading process. */
    SpriteRenderer spriteRenderer; /* Reference to the SpriteRenderer component of the associated GameObject. */
    Color startColor; /* Initial color of the sprite before fading. */

    /**
     * OnStateEnter is called when a transition starts and the state machine starts to evaluate this state.
     * 
     * @param animator The Animator component associated with the GameObject.
     * @param stateInfo The current state information.
     * @param layerIndex The layer index of the state machine.
     */
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
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
        timeElapsed += Time.deltaTime;
        float newAlpha = startColor.a * (1- timeElapsed/fadeTime);
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
    }
}
