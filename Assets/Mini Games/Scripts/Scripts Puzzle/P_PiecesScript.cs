using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/**
 * Author: Aleksandra Rusek
 * 
 * P_PiecesScript class.
 * Handles the behavior of individual puzzle pieces.
 */
public class P_PiecesScript : MonoBehaviour
{
    private Vector3 RightPosition; /* The correct position for the puzzle piece. */
    public bool InRightPosition; /* Flag indicating whether the puzzle piece is in the correct position. */
    public bool Selected; /* Flag indicating whether the puzzle piece is selected by the player. */
    private static int piecesInRightPosition = 0; /* Counter for puzzle pieces in the correct position. */
    private static bool hasWon = false; /* Flag indicating whether the player has won the puzzle. */
    private AudioSource audioSource; /* AudioSource to play sound. */
    [SerializeField] AudioClip collectSound; /* Sound for puzzle in right position. */
    public int puzzleIndex; /* Index of piece. */

    /**
     * Checks if the player has won the puzzle.
     * @return True if the player has won, otherwise false.
     */
    public static bool CheckIfWon()
    {
        return hasWon;
    }

    /**
     * Start is called before the first frame update.
     * This method initializes the puzzle piece's properties, including the correct position, audio source
     * and initial random positioning. It also resets the counters for pieces in the right position and the win flag.
     */
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = 0.2f;
        }
        piecesInRightPosition = 0;
        if (PlayerPrefs.HasKey("PiecesRP"))
        {
            piecesInRightPosition = PlayerPrefs.GetInt("PiecesRP");
        }
        else
        {
            piecesInRightPosition = 0;
        }
        hasWon = false;
        RightPosition = transform.position;
        LoadPuzzleState();
        if(PlayerPrefs.HasKey("PuzzlePlayed"))
        {
            hasWon = true;
        }

    }

    /**
     * Update is called once per frame.
     * Checks if the puzzle piece is in the right position and
     * handles the win condition when all puzzle pieces are in the right position.
     */
    void Update()
    {
        if (Vector3.Distance(transform.position, RightPosition) < 0.5f)
        {
            if (!Selected)
            {
                if (InRightPosition == false)
                {
                    transform.position = RightPosition;
                    InRightPosition = true;
                    GetComponent<SortingGroup>().sortingOrder = 0;
                    if (piecesInRightPosition != 36)
                        audioSource.PlayOneShot(collectSound); 
                    piecesInRightPosition++;                   
                }
                if (piecesInRightPosition == 36)
                {
                    if (!hasWon)
                    {
                        if (!P_TimeManager.CheckIfLost())
                        {
                            hasWon = true;
                            Debug.Log("You win");
                            MG_MGStatus.Instance.GamePassed("PuzzlePlayed");
                        }
                    }
                }
            }
        } 
        if (Input.GetKey(KeyCode.Escape))
            SavePuzzleState();
    }

    /**
     * Saves the puzzle piece's state to PlayerPrefs.
     */
    private void SavePuzzleState()
    {
        PlayerPrefs.SetFloat("PuzzlePosX_" + puzzleIndex, transform.position.x);
        PlayerPrefs.SetFloat("PuzzlePosY_" + puzzleIndex, transform.position.y);
        PlayerPrefs.SetInt("PuzzleInRightPosition_" + puzzleIndex, InRightPosition ? 1 : 0);
        PlayerPrefs.SetInt("PiecesRP", piecesInRightPosition);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("PuzzleGameSaved", 1);
    }
    /**
     * Loads the puzzle piece's state from PlayerPrefs.
     */
    private void LoadPuzzleState()
    {
        if (PlayerPrefs.HasKey("PuzzleGameSaved"))
        {
            float savedPosX = PlayerPrefs.GetFloat("PuzzlePosX_" + puzzleIndex, RightPosition.x);
            float savedPosY = PlayerPrefs.GetFloat("PuzzlePosY_" + puzzleIndex, RightPosition.y);
            InRightPosition = PlayerPrefs.GetInt("PuzzleInRightPosition_" + puzzleIndex, InRightPosition ? 1 : 0) == 1;

            transform.position = new Vector3(savedPosX, savedPosY);
        }
        else
        {
            RightPosition = transform.position;
            transform.position = new Vector3(Random.Range(2f, 13f), Random.Range(-5f, 5f));
        }

    }
    /**
     * Deletes all PlayerPrefs related to puzzle pieces.
     */
    public static void DeleteAllPuzzlePrefs()
    {
        int totalPuzzles = 36; 

        for (int i = 0; i < totalPuzzles; i++)
        {
            PlayerPrefs.DeleteKey("PuzzlePosX_" + i);
            PlayerPrefs.DeleteKey("PuzzlePosY_" + i);
            PlayerPrefs.DeleteKey("PuzzleInRightPosition_" + i);
        }
        PlayerPrefs.DeleteKey("PiecesRP");
        PlayerPrefs.DeleteKey("PuzzleGameSaved");
        piecesInRightPosition = 0;
        PlayerPrefs.Save();
    }

}
