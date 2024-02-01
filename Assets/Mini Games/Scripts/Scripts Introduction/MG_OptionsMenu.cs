using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Aleksandra Rusek
 * 
 * MG_OptionsMenu class.
 * Manages the options menu settings such as volume and difficulty.
 */
public class MG_OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider; /* Reference to the volume slider in the options menu. */
    public Button easyButton; /* Reference to the "Easy" difficulty button. */
    public Button mediumButton; /* Reference to the "Medium" difficulty button. */
    public Button hardButton; /* Reference to the "Hard" difficulty button. */

    /**
     * Start is called before the first frame update.
     * This method initializes the options menu, setting the default difficulty if not present in PlayerPrefs,
     * and retrieves the initial volume value from PlayerPrefs to set on the volume slider.
     */
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Difficulty"))
        {
            SetDifficulty(2f); 
        }
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    /**
     * Saves the current options settings to PlayerPrefs.
     */
    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        float difficultyLevel = PlayerPrefs.GetFloat("Difficulty", 1f);
        Debug.Log("Current difficulty level: " + difficultyLevel);
    }

    /**
     * Called when the volume slider value changes.
     * @param volume The new volume value.
     */
    public void OnVolumeChanged(float volume)
    {
        ApplyVolume(volume);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    /**
     * Applies the volume changes.
     * @param volume The new volume value.
     */
    private void ApplyVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    /**
     * Called when the "Easy" button is clicked.
     */
    public void OnEasyButtonClicked()
    {
        SetDifficulty(1f);
    }

    /**
     * Called when the "Medium" button is clicked.
     */
    public void OnMediumButtonClicked()
    {
        SetDifficulty(2f);
    }

    /**
     * Called when the "Hard" button is clicked.
     */
    public void OnHardButtonClicked()
    {
        SetDifficulty(3f);
    }

    /**
     * Sets the difficulty level.
     * @param difficulty The new difficulty level.
     */
    private void SetDifficulty(float difficulty)
    {
        PlayerPrefs.SetFloat("Difficulty", difficulty);
    }
}
