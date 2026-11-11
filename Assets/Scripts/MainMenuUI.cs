using UnityEngine;
using UnityEngine.SceneManagement;

// Handles navigation from the main menu.
public class MainMenuUI : MonoBehaviour
{
    public async void Play()
    {
        await SceneLoader.Instance.Load(
            "Game",
            Application.exitCancellationToken
        );
    }
}