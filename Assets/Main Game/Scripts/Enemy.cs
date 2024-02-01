using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The Enemy class represents an enemy character with the ability to move towards and attack the player.
 * It includes functionality for movement, detecting the player, and handling attacks.
 */
[RequireComponent(typeof(Rigidbody), typeof(Damageable))]
public class Enemy : MonoBehaviour
{
    public DetectionZone attackZone; /* Reference to the DetectionZone component for detecting the player. */
    public float walkSpeed = 3f; /* Speed at which the enemy walks towards the player. */
    public float walkStopRate = 0.02f; /* Rate at which the enemy slows down when stopping. */
    public Transform player; /* Reference to the player's transform. */
    Rigidbody2D rb; /* Reference to the Rigidbody2D component. */
    Animator animator; /* Reference to the Animator component. */
    Damageable damageable; /* Reference to the Damageable component for handling damage. */

    public bool _hasTarget = false; /* Indicates whether the enemy currently has a target (player). */

    /**
     * Property indicating whether the enemy currently has a target (player).
     */
    public bool HasTarget { get {
            return _hasTarget;
        } private set {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }
    /**
     * Property indicating whether the enemy can move based on its animation state.
     */
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    /**
     * Property representing the remaining cooldown time for the enemy's attack.
     */
    public float AttackCooldown { get {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        } private set { 
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }
    /**
     * Initializes the required components on Awake.
     */
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    /**
     * Update is called once per frame and handles the enemy's behavior, including movement and detection.
     */
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }

        if(player != null)
        {
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            if (!damageable.LockVelocity)
            {
                if (CanMove)
                {
                    transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, walkSpeed * Time.deltaTime);
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
                }
            }
            
            if (CanMove && direction.x < 0)
                transform.localScale = new Vector3(-0.8f, 0.8f, 1);
            else if (CanMove && direction.x > 0)
                transform.localScale = new Vector3(0.8f, 0.8f, 1);
        }

        
    }
    /**
     * Handles the enemy being hit by taking damage and applying knockback.
     * 
     * @param damage Amount of damage received.
     * @param knockback Vector2 representing the knockback force.
     */
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
