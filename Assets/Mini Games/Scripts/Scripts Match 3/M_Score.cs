using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Aleksandra Rusek
 * 
 * Class that manages the scoring system and victory conditions in the Match-3 game.
 */
public class M_Score : MonoBehaviour
{
    [SerializeField] public Text gameWinText; /* Text displaying the win message. */
    public GameObject winParticleSystem; /* Particle system for the win confetti effect. */
    [SerializeField] private Text scoreText; /*  Text displaying the current score. */
    [SerializeField] public int ScoreToBeat = 1000; /* The target score to achieve victory. */
    [SerializeField] private Text scoreToBeatText; /*  Text displaying the target score to beat. */
    [SerializeField] private Image uiFill; /* Image component representing the fill amount for the UI score. */
    private bool winIsPlaying = false; /* Flas indicates if gameWinEffect is playing. */
    private const string ScoreKey = "PlayerScore"; /* Key to save score. */

    /**
    * Singleton instance of the M_Score class.
    */
    public static M_Score Instance { get; private set; }
    private int _score; /** Value representing score */

    /**
    * Property to get or set the current score.
    * Updates the UI elements and checks for victory conditions when the score changes.
    */
    public int Score
    {
        get => _score;

        set
        {
            if (_score == value) return;
            int previousScore = _score;
            _score = value;
            //scoreText.text = "Wynik: " + _score;
            if (uiFill != null)
            uiFill.fillAmount = Mathf.InverseLerp(0, ScoreToBeat, _score);
            PlayerPrefs.SetInt(ScoreKey, _score);
            PlayerPrefs.Save();
            int scoretest = PlayerPrefs.GetInt(ScoreKey);
            Debug.Log("test" + scoretest);
            if (_score > previousScore && _score >= ScoreToBeat)
            {
                GameWin();
            }
        }
    }

    /**
     * Awake method called when the script instance is being loaded.
     * Sets up the singleton instance of the M_Score class.
     */
    private void Awake() => Instance = this;

    /**
     * Start method called before the first frame update.
     * Initializes variables, components, and sets up the UI elements.
     */
    public void Start()
    {
        gameWinText.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey("M3Played"))
        {
            GameWinState();
        }
        winIsPlaying = false;
        GetDifficulty();
        if (scoreToBeatText != null)
        {
            //scoreToBeatText.text = "Wynik do pokonania: " + ScoreToBeat;
            //scoreToBeatText.gameObject.SetActive(true);
        }
        if (PlayerPrefs.HasKey(ScoreKey))
        {
            Score = PlayerPrefs.GetInt(ScoreKey);
        }
    }

    /**
    * Handles the game win effect.
    */
    public void GameWin()
    {
        if (!winIsPlaying && _score >= ScoreToBeat)
        {
            GameWinState();
        }
    }

    /**
     * Handles the game win state.
     */
    public void GameWinState()
    {
        winIsPlaying = true;
        M_WiggleBoss wiggleBossScript = FindObjectOfType<M_WiggleBoss>();
        if (wiggleBossScript != null)
        {
            wiggleBossScript.Explode();
        }
        WinConfetti();
        gameWinText.gameObject.SetActive(true);
        MG_MGStatus.Instance.GamePassed("M3Played");
        PlayerPrefs.DeleteKey(ScoreKey);
        PlayerPrefs.Save();
    }

    /**
     * Plays the win confetti effect.
     */
    private void WinConfetti()
    {

        if (winParticleSystem == null)
            return;
        Vector3 screenCenter = new Vector3((Screen.width / 2f), Screen.height / 3f, 100f);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

        GameObject explosion = Instantiate(winParticleSystem, worldCenter, Quaternion.identity);

        explosion.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        Destroy(explosion, 4f);
    }

    /**
     * Retrieves the difficulty level from player preferences.
     */
    private void GetDifficulty()
    {
        float difficultyLevel = PlayerPrefs.GetFloat("Difficulty", 1f);
        if (difficultyLevel == 1f)
        {
            ScoreToBeat = 1000;
        }
        if (difficultyLevel == 2f)
        {
            ScoreToBeat = 5000;
        }
        if (difficultyLevel == 3f)
        {
            ScoreToBeat = 10000;
        }
    }
}

