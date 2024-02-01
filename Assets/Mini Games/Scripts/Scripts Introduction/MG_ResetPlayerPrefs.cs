using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Aleksandra Rusek
 * 
 * MG_ResetPlayerPrefs class.
 * Resets all PlayerPrefs data.
 */
public class MG_ResetPlayerPrefs : MonoBehaviour
{
    /**
     * Resets all PlayerPrefs data.
     */
    public void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll(); 
        PlayerPrefs.Save(); 
        Debug.Log("PlayerPrefs reset.");
        PlayerPrefs.SetFloat("Difficulty", 2f);
    }
}
