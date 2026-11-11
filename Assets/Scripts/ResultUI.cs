using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestScoreText;

    private void Start()
    {
        // Save the best score if the current score is higher.
        GameSession.SaveBestScore();
        if (scoreText != null)
        {
            int score = Mathf.FloorToInt(GameSession.FinalScore);

            scoreText.text = $"Score: {score}";
        }

        if (bestScoreText != null)
        {
            bestScoreText.text = $"Best Score: {GameSession.BestScore}";
        }
    }


    public async void Retry()
    {
        await SceneLoader.Instance.Load(
            "Game",
            Application.exitCancellationToken
        );
    }

    public async void MainMenu()
    {
        await SceneLoader.Instance.Load(
            "Menu",
            Application.exitCancellationToken
        );
    }
}