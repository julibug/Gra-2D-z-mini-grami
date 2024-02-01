using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

/**
 * Author: Aleksandra Rusek
 * 
 * SC_LineController class.
 * Handles the logic for connecting dots to form a pattern.
 */
public class SC_LineController : MonoBehaviour
{
    private LineRenderer lr; /* LineRenderer component for drawing lines. */
    public List<Transform> points = new List<Transform>(); /* List of selected points to form a line. */
    public Transform lastPoints; /* Last selected point. */
    public SC_Paths[] allPaths; /* Array of predefined paths to match. */
    public SC_ClickPoint clickPoint; /* Reference to the point-clicking logic script. */
    private bool isPatternCompleted = false; /* Flag indicating whether the pattern is completed. */
    public GameObject errorTextObject; /* Error text displayed when the pattern is not completed. */
    public GameObject winTextObject; /* Win text displayed when the pattern is completed. */
    public GameObject winParticleSystem; /* Particle system for winning celebration. */
    public GameObject replayButton; /* Replay button. */
    public GameObject starsNames; /* Names of stars displayed when the pattern is completed. */
    public GameObject moveButton; /* Move to the next scene button. */
    [SerializeField] private AudioClip winSound; /* Sound played when the pattern is completed. */
    [SerializeField] private AudioClip clickSound; /* Sound played when a point is clicked. */
    private AudioSource audioSource; /* AudioSource component for playing sounds. */
    public string moveToScene = null; /* Name of the scene to move to. */
    private int maxCount; /* Maximum count of paths among all predefined paths. */
    [SerializeField] float volumeSound = 0.1f; /* Volume sound level. */
    [SerializeField] bool finalGame = false; /* Flag indicates if its final level of mini game. */

    /**
     * Start is called before the first frame update.
     * Initializes variables, components, and sets up the UI elements.
     */
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = volumeSound;
        }

        winTextObject.SetActive(false);
        errorTextObject.SetActive(false);
        replayButton.SetActive(true);
        starsNames.SetActive(false);
        moveButton.SetActive(false);
        maxCount = MaxCountOfPaths(allPaths);
    }

    /**
     * Awake is called when the script instance is being loaded.
     * Initializes the LineRenderer component.
     */
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    /**
     * Draws a line between points based on the user's input.
     * @param finalPoint The point to connect with the last selected point.
     */
    private void makeLine(Transform finalPoint)
    {
        if (lastPoints == null)
        {
            lastPoints = finalPoint;
            points.Add(lastPoints);
            clickPoint.ChangePointColor(lastPoints);
            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
        }
        else
        {
            if (points.Count < 2 || finalPoint != points[points.Count - 2])
            {
                if (points[points.Count - 1].name != finalPoint.name)
                {
                    points.Add(finalPoint);
                    lr.enabled = true;
                    SetupLine();
                    clickPoint.ChangePointColor(finalPoint);
                    if (audioSource != null && clickSound != null)
                    {
                        audioSource.PlayOneShot(clickSound);
                    }
                }
            }
        }
    }

    /**
     * Sets up the LineRenderer component to draw the line between points.
     */
    private void SetupLine()
    {
        int pointLength = points.Count;
        lr.positionCount = pointLength;
        for (int i = 0; i < pointLength; i++)
        {
            lr.SetPosition(i, points[i].position);
        }
    }

    /**
     * Update is called once per frame.
     * Handles the user input for drawing lines between points and checks if the pattern is completed.
     */
    void Update()
    {
        if (!isPatternCompleted && Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                makeLine(hit.collider.transform);
                bool anyPathEqual = false;

                foreach (SC_Paths paths in allPaths)
                {
                    bool areEqual = points.SequenceEqual(paths.paths);

                    if (areEqual)
                    {
                        anyPathEqual = true;
                        break;
                    }
                }
                if (anyPathEqual)
                {
                    isPatternCompleted = true;
                    WinConfetti();
                    clickPoint.DeactivateClickEffect();
                    winTextObject.SetActive(true);
                    starsNames.SetActive(true);
                    moveButton.SetActive(true);
                    if (finalGame)
                    {
                        MG_MGStatus.Instance.GamePassed("SCPlayed");
                    }
                }
                if (points.Count >= maxCount && anyPathEqual == false)
                {
                    isPatternCompleted = true;
                    errorTextObject.SetActive(true);
                }
            }
        }
    }

    /**
     * Moves to the next scene when called.
     */
    public void MoveNext()
    {
        if (moveToScene != null) SceneManager.LoadScene(moveToScene);
    }

    /**
     * Reloads the current scene to replay the pattern.
     */
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * Calculates the maximum count of paths among all predefined paths.
     * @param all Array of predefined paths.
     * @return The maximum count of paths.
     */
    public int MaxCountOfPaths(SC_Paths[] all)
    {
        int max = 0;

        foreach (SC_Paths scPaths in all)
        {
            int count = scPaths.paths.Count;

            if (count > max)
            {
                max = count;
            }
        }

        return max;
    }

    /**
     * Plays the winning confetti and sound.
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
}
