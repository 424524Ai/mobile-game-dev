using UnityEngine;
using UnityEngine.SceneManagement;

// Handles navigation from the main menu.
public class MainMenuUI : MonoBehaviour
{
    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
}