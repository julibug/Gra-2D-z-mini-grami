using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/**
 * Author: Julia Bugaj
 * 
 * The Clock class manages the display of a timer in a TextMeshPro (TMP) Text component.
 * It calculates the remaining time based on the difference between a fixed duration (1800 seconds) and the stored elapsed time in PlayerPrefs.
 */
public class Clock : MonoBehaviour
{
    public TMP_Text timerText; /* Reference to the TextMeshPro Text component displaying the timer. */

    /**
     * Update is called once per frame.
     * It calculates the remaining time, formats it as "mm:ss", and updates the timerText.
     */
    public void Update()
    {
        float remainingSeconds = (1800f - PlayerPrefs.GetFloat("ElapsedTime"));
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainingSeconds);
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        timerText.text = formattedTime;
    }
}
