using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * Author: Julia Bugaj
 * 
 * The SetBoolBehaviour class extends StateMachineBehaviour and is responsible for setting a boolean parameter during state transitions. 
 */
public class SetBoolBehaviour : StateMachineBehaviour
{
    public string boolName; /* The name of the boolean parameter to be set. */
    public bool updateOnState, updateOnStateMachine; /* Options for updating the boolean parameter on the state or the entire state machine. */
    public bool valueOnEnter, valueOnExit; /* Values to set the boolean parameter to on state enter and exit. */

    /** 
     * OnStateEnter is called before OnStateEnter is called on any state inside this state machine. 
     * 
     * @param animator The Animator component associated with the GameObject.
     * @param stateInfo The current state information.
     * @param layerIndex The layer index of the state machine.
     */
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    /** 
     * OnStateExit is called before OnStateExit is called on any state inside this state machine.
     * 
     * @param animator The Animator component associated with the GameObject.
     * @param stateInfo The current state information.
     * @param layerIndex The layer index of the state machine.
     */
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            animator.SetBool(boolName, valueOnExit);
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
        if (updateOnStateMachine)
        {
            animator.SetBool(boolName, valueOnEnter);
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
        if (updateOnStateMachine)
        {
            animator.SetBool(boolName, valueOnExit);
        }
    }
}
