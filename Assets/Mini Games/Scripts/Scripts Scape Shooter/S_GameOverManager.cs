using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author: Aleksandra Rusek
 * 
 * Manages game over and win conditions.
 */
public class S_GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gamePlay, gameOver, gameWin, winParticleSystem, gameShip, gameEnemySpawner, gamePowerup; /* References to game objects for gameplay, game over, and game win. */
    [SerializeField] private AudioClip winSound; /* The sound played upon winning. */
    private AudioSource audioSource;  /* The audio source component for playing sounds. */
    private bool isGameWin = false; /* Flag indicating whether the game has been won. */
    [SerializeField] float volumeSound = 0.1f; /* Volume sound level. */

    /**
     * Start is called before the first frame update.
     * Initializes references to game objects for gameplay, game over, and game win.
     * Sets the default state of these objects, activates gameplay elements, and ensures the game win flag is false.
     */
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = volumeSound;
        }
        isGameWin = false;
        if (gamePlay != null)
            gamePlay.SetActive(true);
        if (gameOver != null)
            gameOver.SetActive(false);
        if (gameWin != null)
            gameWin.SetActive(false);
        if (gameShip != null)
            gameShip.SetActive(true);
        if (gameEnemySpawner != null)
            gameEnemySpawner.SetActive(true);
        if (gamePowerup != null)
            gamePowerup.SetActive(true);
    }

    /**
     * Activates the game over screen.
     */
    public void GameOver()
    {
        if (isGameWin == true)
            return;
        if (gamePlay != null)
            gamePlay.SetActive(false);
        if (gameOver != null)
            gameOver.SetActive(true);
        if (gameShip != null)
            gameShip.SetActive(false);
        if (gameEnemySpawner != null)
            gameEnemySpawner.SetActive(false);
        if (gamePowerup != null)
            gamePowerup.SetActive(false);
        S_ScoreManager.Instance.deleteKeyScore();
        S_GameStatsManager.Instance.deleteKeyMissiles();
        S_HealthManager healthManager = FindObjectOfType<S_HealthManager>();
        if (healthManager != null)
        {
            healthManager.deleteKeyHealth();
        }
        DestroyEnemiesAndLasersandPlayer();
        S_ScoreManager.Instance.ResetScore();
    }

    /**
     * Activates the game win screen.
     */
    public void GameWin()
    {
        isGameWin = true;
        S_ScoreManager.Instance.deleteKeyScore();
        S_GameStatsManager.Instance.deleteKeyMissiles();
        S_HealthManager healthManager = FindObjectOfType<S_HealthManager>();
        if (healthManager != null)
        {
            healthManager.deleteKeyHealth();
        }
        DestroyEnemiesAndLasersandPlayer();
        WinConfetti();
        if (gamePlay != null)
            gamePlay.SetActive(false);
        if (gameWin != null)
            gameWin.SetActive(true);
        if (gameShip != null)
            gameShip.SetActive(false);
        if (gameEnemySpawner != null)
            gameEnemySpawner.SetActive(false);
        if (gamePowerup != null)
            gamePowerup.SetActive(false);
        MG_MGStatus.Instance.GamePassed("SSPlayed");
        
    }

    /**
     * Reloads the current scene.
     */
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * Loads the scene with the specified index.
     * @param index The index of the scene to be loaded.
     */
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index); 
    }

    /**
     * Spawns confetti particles and plays the win sound.
     */
    private void WinConfetti()
    {
        if (winParticleSystem == null)
            return;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 3f, 0f);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

        GameObject explosion = Instantiate(winParticleSystem, worldCenter, Quaternion.identity);

        explosion.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        Destroy(explosion, 4f);
        if (audioSource != null && winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }
    }

    /**
     * Destroy all objects of type Enemies and Lasers and Player on the scene.
     */
    private void DestroyEnemiesAndLasersandPlayer()
    {
        S_Enemy[] enemies = GameObject.FindObjectsOfType<S_Enemy>();
        foreach (S_Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        S_Laser[] lasers = GameObject.FindObjectsOfType<S_Laser>();
        foreach (S_Laser laser in lasers)
        {
            Destroy(laser.gameObject);
        }

        S_HealthManager[] objectsInGame = GameObject.FindObjectsOfType<S_HealthManager>();
        foreach (S_HealthManager oiG in objectsInGame)
        {
            Destroy(oiG.gameObject);
        }
    }
}
