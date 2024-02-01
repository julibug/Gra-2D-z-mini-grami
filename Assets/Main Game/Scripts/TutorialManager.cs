using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The TutorialManager class manages tutorial pop-ups displayed to guide the player through game mechanics.
 * It handles different tutorials for movement, using a bow and using an ax.
 */
public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps; /* Array of tutorial pop-up GameObjects. */
    private int popUpIndex; /* Index of the current tutorial pop-up. */
    int tutorial; /* Indicating if the general movement tutorial has been completed. */
    int tutorialBow; /* Indicating if the bow tutorial has been completed. */
    int tutorialAx; /* Indicating if the ax tutorial has been completed. */
    MainMenu menu; /* Reference to the MainMenu component. */

    /**
     * Method called when the script instance is being loaded.
     * It retrieves tutorial completion information from PlayerPrefs.
     */
    private void Start()
    {
        tutorial = PlayerPrefs.GetInt("tutorial", 0);
        tutorialBow = PlayerPrefs.GetInt("tutorialBow", 0);
        tutorialAx = PlayerPrefs.GetInt("tutorialAx", 0);
        menu = GameObject.FindObjectOfType<MainMenu>();
    }
    /**
     * Method called every frame.
     * It handles the display of tutorial pop-ups based on player input and inventory items.
     */
    private void Update()
    {
        if (!menu.frozen)
        {
            if (tutorial == 0 && popUps.Length == 3)
            {
                for (int i = 0; i < popUps.Length; i++)
                {
                    if (i == popUpIndex)
                    {
                        popUps[i].SetActive(true);
                    }
                    else
                    {
                        popUps[i].SetActive(false);
                    }
                }

                if (popUpIndex == 0)
                {
                    if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                    {
                        popUpIndex++;
                    }
                }
                else if (popUpIndex == 1)
                {
                    if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift)) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift)))
                    {
                        popUpIndex++;
                    }
                }
                else if (popUpIndex == 2)
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        popUps[2].SetActive(false);
                        tutorial = 1;
                        PlayerPrefs.SetInt("tutorial", tutorial);
                    }
                }
            }

            if (tutorialBow == 0 && Inventory.instance.HasBow() && popUps[0].name == "PopUpBow")
            {
                popUps[0].SetActive(true);
                if (Input.GetKey(KeyCode.Mouse2))
                {
                    popUps[0].SetActive(false);
                    tutorialBow = 1;
                    PlayerPrefs.SetInt("tutorialBow", tutorialBow);
                }
            }

            if (tutorialAx == 0 && Inventory.instance.HasAx() && popUps[0].name == "PopUpAx")
            {
                popUps[0].SetActive(true);
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    popUps[0].SetActive(false);
                    tutorialAx = 1;
                    PlayerPrefs.SetInt("tutorialAx", tutorialAx);
                }
            }
        }
    }
}
