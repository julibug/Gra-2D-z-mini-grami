using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Author: Julia Bugaj
 * 
 * The PlayerController class handles player movement, actions and interactions.
 */
[RequireComponent(typeof(Rigidbody), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f; /* Walk speed of the player. */
    private bool lost = false; /* Indicating if the player has lost the game. */

    Vector2 moveInput; /* Input for player movement. */
    Damageable damageable; /* Reference to the Damageable component. */

    /**
     * Current movement speed, considering if the player can move.
     */
    public float CurrentMoveSpeed { get {
            if (CanMove)
            {
                return walkSpeed;
            } else
            {
                return 0;
            }
        } private set {
        
        } }

    [SerializeField]
    private bool _isMoving = false; /* Indicating if the player is currently moving. */

    /** 
     * IsMoving property.
     * Indicates whether the object is currently in motion.
     * Allows reading the moving status and internally sets the value.
     */
    public bool IsMoving { get {
            return _isMoving;
        } private set {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } }

    public bool _isFacingLeft = true; /* Indicating if the player is facing left. */

    /**
     * Property for checking if the player is facing left.
     */
    public bool isFacingLeft { get {
            return _isFacingLeft;
        } private set {
            if (_isFacingLeft != value) { 
                transform.localScale *= new Vector2(-1,1);
            }
            _isFacingLeft = value;

        } 
    }
    /**
     * Property for checking if the player can move.
     */
    public bool CanMove { get {
            return animator.GetBool(AnimationStrings.canMove);
        } }

    Rigidbody2D rb; /* Reference to the Rigidbody2D component. */
    Animator animator; /* Reference to the Animator component. */

    /**
     * Initializes references to components.
     */
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    /**
    * Update is called once per frame. It checks if the player is alive and initiates specific logic when the player dies.
    */
    void Update()
    {
        if (IsAlive == false && lost == false) {
            lost = true;
            StartCoroutine(Dead());
            Timer.instance.isTimerActive = false;
        }
    }
    /**
     * It ensures that the player's velocity is updated based on input and movement restrictions.
     */
    private void FixedUpdate()
    {
        if (!damageable.LockVelocity) { 
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }
    }
    /**
     * Coroutine to handle the player's death.
     */
    private IEnumerator Dead()
    {
        yield return new WaitForSeconds(1.5f);
        SceneController.instance.LoadLostMenu();
    }
    /**
     * Input handler for player movement.
     */
    public void OnMove(InputAction.CallbackContext context) { 
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        } else
        {
            IsMoving = false;
        }
    }
    /**
     * Property indicating if the player is alive.
     */
    public bool IsAlive
    {
        get {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }
    /**
     * Sets the facing direction of the player based on the movement input.
     */
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && isFacingLeft)
        {
            isFacingLeft = false;
        } else if (moveInput.x < 0 && !isFacingLeft)
        {
            isFacingLeft = true;
        }
    }
    /**
     * Input handler for player attack.
     */
    public void OnAttack(InputAction.CallbackContext context) {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
    /**
     * Input handler for ranged attack.
     */
    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Inventory.instance.HasBow())
            {
                animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
            }
        }
    }
    /**
     * Input handler for player hit.
     */
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
    /**
     * Input handler for player dodge.
     */
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.started && IsMoving)
        {
            animator.SetTrigger(AnimationStrings.dodgeTrigger);
            damageable.Dodge();
        }
        
    }
    /**
     * Input handler for ax attack.
     */
    public void OnAxAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Inventory.instance.HasAx())
            {
                animator.SetTrigger(AnimationStrings.axAttackTrigger);
            }
        }
    }
}
