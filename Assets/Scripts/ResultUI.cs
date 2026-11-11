using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        if (scoreText != null)
        {
            int score = Mathf.FloorToInt(GameSession.FinalScore);

            scoreText.text = $"Score: {score}";
        }
    }


    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
}