using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Aleksandra Rusek
 * 
 * P_RandomPuzzle class.
 * Sets up a random puzzle by assigning a chosen image to puzzle pieces.
 */
public class P_RandomPuzzle : MonoBehaviour
{
    public GameObject Canvas; /* Reference to the canvas containing the puzzle pieces. */
    public Image[] puzzleImages; /* Array of puzzle images to choose from. */

    /**
     * Start is called before the first frame update.
     * This method initializes the random puzzle setup by selecting a random image from the array to assign to the puzzle pieces.
     */
    void Start()
    {
        Canvas.SetActive(false);
        if (PlayerPrefs.HasKey("SelectedImage"))
        {
            string selectedImageName = PlayerPrefs.GetString("SelectedImage");
            Image selectedImage = System.Array.Find(puzzleImages, img => img.name == selectedImageName);
            if (selectedImage != null)
            {
                SetPuzzles(selectedImage);
            }
        }
        else
        {
            SetPuzzles(puzzleImages[Random.Range(0, puzzleImages.Length)]);
        }
    }

    /**
     * Sets the puzzle pieces with the provided image.
     * @param Photo The image to assign to puzzle pieces.
     */
    public void SetPuzzles(Image Photo)
    {

        PlayerPrefs.SetString("SelectedImage", Photo.name);
        for (int i = 0; i < 36; i++)
        {
            GameObject puzzlePiece = GameObject.Find("Piece (" + i + ")");
            puzzlePiece.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = Photo.sprite;
        }



    }

    /**
     * Delete player prefab.
     */
    public static void DeleteImageKey()
    {
        PlayerPrefs.DeleteKey("SelectedImage");
    }

}
