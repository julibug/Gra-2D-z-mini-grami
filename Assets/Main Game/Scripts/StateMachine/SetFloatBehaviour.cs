using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * Author: Julia Bugaj
 * 
 * The SetFloatBehaviour class extends StateMachineBehaviour and is responsible for setting a float parameter during state transitions. 
 */
public class SetFloatBehaviour : StateMachineBehaviour
{
    public string floatName; /* The name of the float parameter to be set. */
    public bool updateOnStateEnter, updateOnStateExit; /* Indicates whether to update the float parameter on entering or exiting the state. */
    public bool updateOnStateMachineEnter, updateOnStateMachineExit; /* Indicates whether to update the float parameter on entering or exiting the state machine. */
    public float valueOnEnter, valueOnExit; /* Values to set the float parameter to on state enter and exit. */

    /** 
     * OnStateEnter is called when a transition starts and the state machine starts to evaluate this state. 
     *
     * @param animator The Animator component associated with the GameObject.
     * @param stateInfo The current state information.
     * @param layerIndex The layer index of the state machine.*/
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnStateEnter)
        {
            animator.SetFloat(floatName, valueOnEnter);
        }
    }

    /** 
     * OnStateExit is called when a transition ends and the state machine finishes evaluating this state.
     * 
     * @param animator The Animator component associated with the GameObject.
     * @param stateInfo The current state information.
     * @param layerIndex The layer index of the state machine.
     */
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnStateExit)
        {
            animator.SetFloat(floatName, valueOnExit);
        }
    }

    /** 
     * OnStateMachineEnter is called when entering a state machine via its Entry Node. 
     *
     * @param animator The Animator component associated with the GameObject.
     * @param stateMachinePathHash The hash of the path the state machine takes.
     */
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachineEnter)
        {
            animator.SetFloat(floatName, valueOnEnter);
        }
    }

    /**
     * OnStateMachineExit is called when exiting a state machine via its Exit Node. 
     * 
     * @param animator The Animator component associated with the GameObject.
     * @param stateMachinePathHash The hash of the path the state machine takes.
     */
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachineExit)
        {
            animator.SetFloat(floatName, valueOnExit);
        }
    }
}
