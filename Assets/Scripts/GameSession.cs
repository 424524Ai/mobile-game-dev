using UnityEngine;

// Stores the current score and persistent best score.
public static class GameSession
{
    public static int FinalScore{ get; set; }

    public static int BestScore =>
        PlayerPrefs.GetInt("BestScore", 0);

    public static void SaveBestScore()
    {
        if (FinalScore > BestScore)
        {
            PlayerPrefs.SetInt("BestScore", FinalScore);
            PlayerPrefs.Save();
        }
    }
}
