using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * Author: Aleksandra Rusek
 * 
 * P_TimeManager class.
 * Manages the time, UI, and game state for a puzzle game.
 */
public class P_TimeManager : MonoBehaviour
{
    public Text timerText;  /* Text displaying the remaining time. */
    public Text gameOverText; /* Text displayed when the game is over. */
    public Text gameWinText; /* Text displayed when the player wins. */
    public Button gameNextButton; /* Button to proceed to the next scene. */
    public Button replayButton; /* Button to replay the current scene. */
    public GameObject winParticleSystem; /* Particle system for winning celebration. */
    [SerializeField] private Image uiFill;  /* Image representing the time fill in UI. */
    [SerializeField] private Text uiText; /* Text displaying the formatted time in UI. */
    [SerializeField] private AudioClip winSound; /* Sound played when the player wins. */
    private AudioSource audioSource; /* AudioSource for playing sounds. */
    public float gameTime = 30f; /* Total time allowed for the puzzle. */
    private float timer; /* Current time elapsed. */
    private static bool isGameOver = false; /* Flag indicating whether the game is over. */
    private bool wasConfettiPlayed = false; /* Flag indicating whether confetti was already played. */
    private const string PlayerPrefsKey = "RemainingTimePuzzle"; /* Key to store remaining puzzle time. */

    /**
    * Awake is called when the script instance is being loaded.
    * Set up initial UI state.
    */
    private void Awake()
    {
        gameNextButton.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        replayButton.onClick.AddListener(Replay);
        gameWinText.gameObject.SetActive(false);
        wasConfettiPlayed = false;
    }

    /**
     * Start is called before the first frame update.
     * Initialize timer and UI.
     */
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = 0.2f;
        }
        getDifficulty();
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            timer = PlayerPrefs.GetFloat(PlayerPrefsKey);
        }
        else
        {
            timer = 0f;
        }
        UpdateTimerDisplay();
        isGameOver = false;
    }

    /**
     * Update is called once per frame.
     * Check if the player has won and update timer if the game is not over.
     */
    void Update()
    {
        if (P_PiecesScript.CheckIfWon())
        {
            WinConfetti();
            gameNextButton.gameObject.SetActive(true);
            return;
        }
        if (!isGameOver)
        {
            timer += Time.deltaTime;

            if (timer >= gameTime && P_PiecesScript.CheckIfWon() == false)
            {
                GameOver();
            }

            UpdateTimerDisplay();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            PlayerPrefs.SetFloat(PlayerPrefsKey, timer);
            PlayerPrefs.Save();
        }
    }

    /**
    * Checks if the player has lost.
    * @return True if the player has los, otherwise false.
    */
    public static bool CheckIfLost()
    {
        return isGameOver;
    }

    /**
     * Updates the timer display in UI.
     */
    void UpdateTimerDisplay()
    {
        float timeLeft = gameTime - Mathf.Round(timer);
        if (timerText != null)
        {
            timerText.text = "Time left: " + timeLeft.ToString() + " s";
        }
        if (uiText != null && uiFill != null)
        {
            uiText.text = $"{(int)timeLeft / 60:00} : {timeLeft % 60:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, gameTime, timeLeft);
        }
    }

    /**
     * Handles the game over state.
     */
    void GameOver()
    {

        gameOverText.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true); 
        isGameOver = true;
        PlayerPrefs.DeleteKey(PlayerPrefsKey);
        P_PiecesScript.DeleteAllPuzzlePrefs();
        P_RandomPuzzle.DeleteImageKey();
        PlayerPrefs.Save();
    }

    /**
     * Reloads the current scene to replay the game.
     */
    void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
    * Plays the winning confetti and sound.
    */
    private void WinConfetti()
    {
        if (wasConfettiPlayed == false)
        {
            PlayerPrefs.DeleteKey(PlayerPrefsKey);
            P_PiecesScript.DeleteAllPuzzlePrefs();
            P_RandomPuzzle.DeleteImageKey();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefsKey deleted: " + !PlayerPrefs.HasKey(PlayerPrefsKey));
            Debug.Log("PiecesPrefs deleted: " + !PlayerPrefs.HasKey("PuzzlePosX_0"));  
            Debug.Log("ImageKey deleted: " + !PlayerPrefs.HasKey("ImageKey"));
            if (winParticleSystem == null)
                return;
            wasConfettiPlayed = true;
            Vector3 screenCenter = new Vector3((Screen.width / 2f), Screen.height / 3f, 100f);
            Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

            GameObject explosion = Instantiate(winParticleSystem, worldCenter, Quaternion.identity);

            explosion.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            Destroy(explosion, 4f);

            if (audioSource != null && winSound != null)
            {
                audioSource.PlayOneShot(winSound);
            }
        }
    }

    /**
     * Retrieves the difficulty  from player preferences and sets the game time accordingly.
     */
    private void getDifficulty()
    {
        float difficultyLevel = PlayerPrefs.GetFloat("Difficulty", 1f);
        Debug.Log("Aktualny poziom trudnoœci: " + difficultyLevel);
        if(difficultyLevel == 1f)
        {
            gameTime = 300f;
        }
        if(difficultyLevel == 2f)
        {
            gameTime = 180f;
        }
        if (difficultyLevel == 3f)
        {
            gameTime = 100f;
        }
    }
}
