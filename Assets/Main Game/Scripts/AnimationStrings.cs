using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Author: Julia Bugaj
 * 
 * The AnimationStrings class provides static string constants representing various animation parameters and triggers.
 * These strings are commonly used to access and control animations in the character controller.
 */
internal class AnimationStrings
{
    internal static string isMoving = "isMoving"; /* Animation parameter for character movement. */
    internal static string attackTrigger = "attack"; /* Animation trigger for initiating an attack. */
    internal static string canMove = "canMove"; /* Animation parameter controlling whether the character can move. */
    internal static string hasTarget = "hasTarget"; /* Animation parameter indicating whether the character has a target. */
    internal static string isAlive = "isAlive"; /* Animation parameter representing the character's life state. */
    internal static string hitTrigger = "hit"; /* Animation trigger for registering a hit. */
    internal static string lockVelocity = "lockVelocity"; /* Animation parameter for locking character velocity. */
    internal static string attackCooldown = "attackCooldown"; /* Animation parameter representing the cooldown state of the attack. */
    internal static string rangedAttackTrigger = "rangedAttack"; /* Animation trigger for initiating a ranged attack. */
    internal static string dodgeTrigger = "dodge"; /* Animation trigger for executing a dodge. */
    internal static string axAttackTrigger = "axAttack"; /* Animation trigger for initiating an ax attack. */
}
