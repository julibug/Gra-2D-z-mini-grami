using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Author: Julia Bugaj
 * 
 * The Damageable class manages the health and damage-related functionality of a game object.
 * It handles taking damage, healing, dodging and spawning items on death.
 */
public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit; /* Event invoked when the object is hit, passing damage and knockback information. */
    public UnityEvent<int, int> healthChange; /* Event invoked when the health changes, passing current health and max health. */
    public GameObject itemToSpawn; /* Item to spawn upon death. */
    public Transform spawnPoint; /* Spawn point for the item upon death. */
    Animator animator; /*Reference to the Animator component for controlling object animations.*/
    [SerializeField]
    private int _maxHealth = 100; /* Maximum health of the object.*/
    /**
     * Maximum health property.
     * Allows reading the maximum health value and internally sets the value.
     */
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        private set
        {
            _maxHealth = value;
        }
    }
    [SerializeField]
    private int _health = 100; /*Current health of the object.*/
    /** 
     * Object's health property.
     * Allows reading the current health value and controls additional operations when setting a new value.
     */
    public int Health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = (value < 0) ? 0 : value;
            healthChange?.Invoke(_health, MaxHealth);
            if (_health <= 0)
            {
                IsAlive = false;
                if (itemToSpawn != null && !Inventory.instance.HasKey(itemToSpawn.name)) { 
                    Instantiate(itemToSpawn, spawnPoint.position, Quaternion.identity); 
                }
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true; /*Indicating if the object is currently alive.*/
    [SerializeField]
    private bool canBeHit = true; /*Indicating if the object can be hit.*/


    private float timeSinceHit = 0f; /*Time elapsed since the last hit.*/
    public float cannotBeHitTime = 0.25f; /*Duration during which the object cannot be hit.*/

    /**
     * Property representing whether the object is alive or not.
     */
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        private set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set " + value);
        }
    }

    /**
     * Property representing whether the object's velocity is locked.
     */
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }
    /**
     * Start is called before the first frame update.
     * It retrieves the saved current health from PlayerPrefs and sets the initial health.
     */
    private void Start()
    {
        int objectCurrentHealth = PlayerPrefs.GetInt(gameObject.name + "CurrentHealth", 100);
        Health = objectCurrentHealth;
    }
    /**
     * Awake is called when the script instance is being loaded.
     * It initializes the animator component.
     */
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    /**
     * Update is called once per frame.
     * It handles the cooldown for being hit and saves the current health to PlayerPrefs.
     */
    private void Update()
    {
        if (!canBeHit)
        {
            if (timeSinceHit > cannotBeHitTime)
            {
                canBeHit = true;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
        PlayerPrefs.SetInt(gameObject.name + "CurrentHealth", Health);
    }
    /**
     * Hit function deals damage to the object and triggers hit-related events.
     * It also sets the object as not hittable for a short duration.
     * 
     * @param damage The amount of damage.
     * @param knockback The knockback vector to apply.
     * @return True if the hit was successful, false otherwise.
     */
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && canBeHit)
        {
            Health -= damage;
            canBeHit = false;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterActions.characterDamaged.Invoke(gameObject, damage);
            return true;
        }
        return false;
    }
    /**
     * Heal function restores the object's health up to the maximum health.
     * It triggers healing-related events.
     */
    public void Heal()
    {
        if (IsAlive)
        {
            int healthRestored = MaxHealth - Health;
            if (healthRestored > 0)
            {
                Health += healthRestored;
                CharacterActions.characterHealed.Invoke(gameObject, healthRestored);
            }
        }
    }

    /**
     * Dodge function makes the object temporarily invulnerable to hits.
     */
    public void Dodge()
    {
        if(IsAlive)
        {
            canBeHit = false;
        }
    }
}
