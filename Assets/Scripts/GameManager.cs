using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        Playing,
        Paused,
        Won,
        Lost
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            SetState(State.Lost);
        }
    }

    public State CurrentState {  get; private set; } = State.Playing;
    public event Action<State> StateChanged;

    public void SetState(State newState)
    {
        if (CurrentState == newState)
        {
            return;
        }
        CurrentState = newState;
        Debug.Log("Game State: " + CurrentState);
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
