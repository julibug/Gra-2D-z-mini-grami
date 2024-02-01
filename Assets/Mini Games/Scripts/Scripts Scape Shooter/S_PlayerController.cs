using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Aleksandra Rusek
 * 
 * Controls the player's movement and firing mechanics.
 */
public class S_PlayerController : MonoBehaviour
{
    public static S_PlayerController Instance; /* Singleton instance of the player controller. */

    /**
    * Enum representing the type of movement input for the player.
    */
    public enum MovementInputType
    {
        PointerBased, ButtonBased
    }
    [SerializeField] private MovementInputType movementInputType; /** The singleton instance of the player controller. */
    [SerializeField] private KeyCode up = KeyCode.UpArrow, down = KeyCode.DownArrow, left = KeyCode.LeftArrow, right = KeyCode.RightArrow; /** The key codes for movement directions. */
    [SerializeField] private float Speed = 10f; /** The movement speed of the player. */
    [SerializeField] private Vector2 minPos, maxPos; /** The boundaries for the player's movement. */
    private Vector2 pos; /** The position of the player. */

    [Header("Laser")]
    [SerializeField] private GameObject laser; /** The laser prefab. */
    [SerializeField] private Vector2 laserSpeed = new Vector2(0f, 1f); /** The speed of the laser. */
    [SerializeField] private Vector3 spawnOffset; /** The offset for spawning the laser. */
    [SerializeField] private float laserFireRate = 0.2f; /** The firing rate of the laser. */
    [SerializeField] private KeyCode laserKey = KeyCode.Mouse1; /** The key code for firing the laser. */

    private S_ObjectPool laserPool; /** The object pool for lasers. */
    [SerializeField] private int laserPoolsize = 30; /** The size of the laser object pool. */

    [Header("Missile")]
    [SerializeField] private GameObject missile; /** The missile prefab. */
    [SerializeField] private Vector2 missileSpeed = new Vector2(0f, 0.5f); /** The speed of the missile. */
    [SerializeField] private Vector3 spawnOffsetMissile; /** The offset for spawning the missile. */
    [SerializeField] private KeyCode missileKey = KeyCode.Mouse1; /** The key code for firing the missile. */

    private S_ObjectPool missilePool; /** The object pool for missiles. */
    [SerializeField] private int missilePoolsize = 30; /** The size of the missile object pool. */

    /**
     * Start is called before the first frame update.
     * Initializes the object pool for lasers and missiles.
     */
    void Start()
    {
        
        Instance = this;
        laserPool = new S_ObjectPool(laser, laserPoolsize, "PlayerLaserPool");
        missilePool = new S_ObjectPool(missile, missilePoolsize, "PlayerMissilePool"); 
    }

    /**
     * Release the laser back to the object pool.
     */
    public void ReleaseLaser(GameObject laser)
    {
        laserPool.ReturnInstance(laser);
    }

    /**
     * Release the missile back to the object pool.
     */
    public void ReleaseMissile(GameObject missile)
    {
        missilePool.ReturnInstance(missile);
    }

    /**
     * Fire the laser.
     */
    private void Fire()
    {
        if (S_GameStatsManager.Instance.CheckIfCanShootLaser(1))
        {
            GameObject laserInstance = laserPool.GetInstance();
            laserInstance.transform.position = transform.position + spawnOffset;
            laserInstance.GetComponent<Rigidbody2D>().AddForce(laserSpeed, ForceMode2D.Impulse);
            S_GameStatsManager.Instance.ShootLasersByAmount(1);
        }
    }

    /**
     * Fire the missile.
     */
    private void MissileFire()
    {
        if (S_GameStatsManager.Instance.CheckIfCanShootMissiles(1))
        {
            GameObject missileInstance = missilePool.GetInstance();
            missileInstance.transform.position = transform.position + spawnOffsetMissile;
            missileInstance.GetComponent<Rigidbody2D>().AddForce(missileSpeed, ForceMode2D.Impulse);
            S_GameStatsManager.Instance.ShootMissilesByAmount(1);
        }
    }

    /**
     * Update is called once per frame.
     * Handles player input, movement, and firing mechanisms.
     */
    void Update()
    {
        Instance = this;
        if (movementInputType == MovementInputType.ButtonBased)
        {
 #if UNITY_STANDALONE || UNITY_WEBGL
            if(Input.GetKey(up))
            {
                transform.Translate(Speed * Vector2.up * Time.deltaTime);
            }
            else if (Input.GetKey(down))
            {
                transform.Translate(Speed * Vector2.down * Time.deltaTime);
            }
            if (Input.GetKey(left))
            {
                transform.Translate(Speed * Vector2.left * Time.deltaTime);
            }
            else if (Input.GetKey(right))
            {
                transform.Translate(Speed * Vector2.right * Time.deltaTime);
            }
#endif
        }
        else
        {
            Vector3 rawPos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(rawPos);
            if (Input.GetKey(KeyCode.Mouse0))
                transform.position = Vector3.Lerp(transform.position, worldPos, Speed * Time.deltaTime);
        }
#if UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetKeyDown(laserKey))
            InvokeRepeating("Fire", 0.001f, laserFireRate);

        if (Input.GetKeyUp(laserKey))
            CancelInvoke("Fire");

        if (Input.GetKeyDown(missileKey))
            MissileFire();
#endif
        pos.x = Mathf.Clamp(transform.position.x, minPos.x, maxPos.x);
        pos.y = Mathf.Clamp(transform.position.y, minPos.y, maxPos.y);

        transform.position = pos;

    }
}
