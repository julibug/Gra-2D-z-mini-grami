using System.IO;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Text;

/**
 * Author: Aleksandra Rusek
 * 
 * MG_HighscoreManager class.
 * Manages the highscores of game.
 */
public class MG_HighscoreManager : MonoBehaviour
{
    private string highscorePathEasy; /* Path to the file for easy difficulty */
    private string highscorePathMedium; /* Path to the file for medium difficulty */
    private string highscorePathHard; /* Path to the file for hard difficulty */
    [SerializeField] public TextMeshProUGUI text; /* Text displaying highscores */
    [System.Serializable]
    public class HighscoreEntry
    {
        public float difficultyLevel; /* Difficulty level */
        public float time; /* Time achieved by the player */

        /**
         * Constructor for the HighscoreEntry class.
         * @param level Difficulty level entered by the player.
         * @param t Time achieved by the player.
         */
        public HighscoreEntry(float level, float t)
        {
            difficultyLevel = level;
            time = t;
        }
    }
    
    private List<HighscoreEntry> highscoresEasy; /* List of highscores for easy difficulty */
    private List<HighscoreEntry> highscoresMedium; /* List of highscores for medium difficulty */
    private List<HighscoreEntry> highscoresHard; /* List of highscores for hard difficulty */

    /** Singleton instance of the MG_HighscoreManager class. */
    public static MG_HighscoreManager Instance { get; private set; }

    /** 
     * Start method to load paths
     */
    public void Start()
    {
        LoadPaths();
        if (highscoresEasy == null)
            highscoresEasy = new List<HighscoreEntry>();

        if (highscoresMedium == null)
            highscoresMedium = new List<HighscoreEntry>();

        if (highscoresHard == null)
            highscoresHard = new List<HighscoreEntry>();
        LoadHighscores();
    }
    /**
     * Method for loading highscores from files.
     */
    void LoadHighscores()
    {
        if (File.Exists(highscorePathEasy))
            LoadHighscoresFromFile(highscorePathEasy, highscoresEasy);

        if (File.Exists(highscorePathMedium))
            LoadHighscoresFromFile(highscorePathMedium, highscoresMedium);

        if (File.Exists(highscorePathHard))
            LoadHighscoresFromFile(highscorePathHard, highscoresHard);
    }

    /**
     * Method for loading highscores from a file.
     * @param filePath Path to the file.
     * @param highscoreList List to which the scores will be added.
     */
    void LoadHighscoresFromFile(string filePath, List<HighscoreEntry> highscoreList)
    {
        highscoreList.Clear();
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] data = line.Split(';');
                if (true)
                {
                    float level = float.Parse(data[0]);
                    float time = float.Parse(data[1]);
                    Debug.Log($"Loaded entry: Level {level}, Time {time}");
                    highscoreList.Add(new HighscoreEntry(level, time));
                }
            }
            highscoreList.Sort((x, y) => x.time.CompareTo(y.time));
        }
        catch (IOException e)
        {
            Debug.LogError($"Error reading from file {filePath}: {e.Message}");
        }
    }

    /**
     * Method for saving a score to a file.
     * @param level Difficulty level.
     * @param time Time achieved by the player.
     */
    public void SaveHighscore(float level, float time)
    {
        HighscoreEntry newEntry = new HighscoreEntry(level, time);

        if (level == 1)
            SaveHighscoreToFile(newEntry, highscorePathEasy, highscoresEasy);
        else if (level == 2)
            SaveHighscoreToFile(newEntry, highscorePathMedium, highscoresMedium);
        else if (level == 3)
            SaveHighscoreToFile(newEntry, highscorePathHard, highscoresHard);

        DisplayHighscores();
    }

    /**
     * Method for saving a score to a file.
     * @param entry Score to be saved.
     * @param filePath Path to the file.
     * @param highscoreList List to which the score will be added.
     */
    void SaveHighscoreToFile(HighscoreEntry entry, string filePath, List<HighscoreEntry> highscoreList)
    {
        highscoreList.Add(entry);
        highscoreList.Sort((x, y) => x.time.CompareTo(y.time));
        highscoreList.Reverse();

        if (highscoreList.Count > 10)
        {
            highscoreList.RemoveAt(10);
        }

        SaveHighscoresToFile(filePath, highscoreList);
    }

    /**
    * Method for saving scores to a file.
    * @param filePath Path to the file.
    * @param highscoreList List of scores to be saved.
    */
    void SaveHighscoresToFile(string filePath, List<HighscoreEntry> highscoreList)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (HighscoreEntry entry in highscoreList)
                {
                    writer.WriteLine($"{entry.difficultyLevel};{entry.time};");
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error writing to file {filePath}: {e.Message}");
        }
    }

    /**
     * Method for displaying highscores.
     */
    public void DisplayHighscores()
    {
        LoadPaths();
        if (highscoresEasy == null)
            highscoresEasy = new List<HighscoreEntry>();

        if (highscoresMedium == null)
            highscoresMedium = new List<HighscoreEntry>();

        if (highscoresHard == null)
            highscoresHard = new List<HighscoreEntry>();
        LoadHighscores();
        if (text != null)
        {
            text.text = string.Empty; 

            DisplayHighscoresForDifficulty(highscoresEasy, "³atwego");
            DisplayHighscoresForDifficulty(highscoresMedium, "œredniego");
            DisplayHighscoresForDifficulty(highscoresHard, "trudnego");
        }
    }

    /**
     * Method for displaying highscores for a specific difficulty level.
     * @param highscoreList List of highscores for the difficulty level.
     * @param difficultyName Name of the difficulty level to be displayed in the header.
     */
    void DisplayHighscoresForDifficulty(List<HighscoreEntry> highscoreList, string difficultyName)
    {
        if (text != null)
        {
            if (highscoreList != null)
            {
                string currentText = text.text; 

                StringBuilder displayText = new StringBuilder(currentText);
                if (highscoreList.Count > 0) 
                {
                    displayText.AppendLine($"Najlepsze wyniki dla poziomu {difficultyName}:");
                } else
                {
                    displayText.AppendLine($"Brak wyników dla poziomu {difficultyName}.");
                    text.text = displayText.ToString();
                    return;
                }

                for (int i = 0; i < Mathf.Min(highscoreList.Count, 3); i++)
                {
                    float timeInMinutes = highscoreList[i].time / 60f;
                    int minutes = Mathf.FloorToInt(timeInMinutes);
                    int seconds = Mathf.FloorToInt(highscoreList[i].time - minutes * 60f);
                    displayText.AppendLine($"{i + 1}. Czas: {minutes:00}:{seconds:00}");
                }
                    text.text = displayText.ToString();
            }
        }
    }

    /**
     * Method to load paths of score rank.
     */
    private void LoadPaths()
    {
        Debug.Log("Persistant Data Path: " + Application.persistentDataPath);
        if (Instance == null)
        {
            Instance = this;
            highscorePathEasy = Application.persistentDataPath + "/highscores_easy.txt";
            highscorePathMedium = Application.persistentDataPath + "/highscores_medium.txt";
            highscorePathHard = Application.persistentDataPath + "/highscores_hard.txt";
            if (!File.Exists(highscorePathEasy))
            {
                File.Create(highscorePathEasy);
            }

            if (!File.Exists(highscorePathMedium))
            {
                File.Create(highscorePathMedium);
            }

            if (!File.Exists(highscorePathHard))
            {
                File.Create(highscorePathHard);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
