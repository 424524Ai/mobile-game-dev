using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        Playing,
        Paused,
        Won,
        Lost
    }

    public State CurrentState {  get; private set; } = State.Playing;
    public event Action<State> StateChanged;

    // Tracks the current attempt and its gameplay duration.
    // Counts gameplay attempts during the current application session.
    private static int attempt = 0;
    private float levelStartTime;
    // Records total gameplay earnings when the attempt begins.
    private int gameplayCoinsAtStart;
    private Wallet wallet;

    private void Start()
    {
        wallet = FindFirstObjectByType<Wallet>();

        attempt++;
        levelStartTime = Time.time;
        gameplayCoinsAtStart = wallet != null ? wallet.GameplayCoinsEarned : 0;

        // Record the start of a gameplay attempt.
        Telemetry.Log("level_start",
            ("level_id", 1),
            ("attempt", attempt),
            ("coins", wallet != null ? wallet.Coins : 0));
    }

    public void SetState(State newState)
    {
        if (CurrentState == newState)
        {
            return;
        }
        CurrentState = newState;
        Debug.Log("Game State: " + CurrentState);

        if (newState == State.Lost)
        {
            // Record the result of a failed gameplay attempt.
            float elapsedTime = Time.time - levelStartTime;
            // Calculate coins collected during this attempt, excluding purchases.
            int coinsEarned = wallet != null
                ? wallet.GameplayCoinsEarned - gameplayCoinsAtStart
                : 0;

            Telemetry.Log("level_fail",
                ("level_id", 1),
                ("time_s", elapsedTime.ToString("F2")),
                ("cause", "obstacle"),
                ("coins_earned", coinsEarned));
        }

        if (newState == State.Won)
        {
            // Record a successfully completed gameplay attempt.
            float elapsedTime = Time.time - levelStartTime;
            // Calculate coins collected during this attempt, excluding purchases.
            int coinsEarned = wallet != null
                ? wallet.GameplayCoinsEarned - gameplayCoinsAtStart
                : 0;

            Telemetry.Log("level_complete",
                ("level_id", 1),
                ("time_s", elapsedTime),
                ("coins_earned", coinsEarned),
                ("score", 0));
        }

        StateChanged?.Invoke(newState);

        if (newState == State.Won || newState == State.Lost)
        {
            LoadResultAsync();
        }
    }

    private void OnEnable()
    {
        LifecycleGuard.PausedChanged += OnPauseChanged;
        OnPauseChanged(LifecycleGuard.IsPaused);
    }

    private void OnDisable()
    {
        LifecycleGuard.PausedChanged -= OnPauseChanged;
    }

    private void OnPauseChanged(bool paused)
    {
        if (paused)
        {
            if (CurrentState == State.Playing)
            {
                SetState(State.Paused);
            }
        }
        else
        {
            if (CurrentState == State.Paused)
            {
                SetState(State.Playing);
            }
        }
    }

    private async void LoadResultAsync()
    {
        await SceneLoader.Instance.Load(
            "Result",
            Application.exitCancellationToken
        );
    }
}
