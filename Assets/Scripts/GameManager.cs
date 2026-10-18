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

    public void SetState(State newState)
    {
        if (CurrentState == newState)
        {
            return;
        }
        CurrentState = newState;
        Debug.Log("Game State: " + CurrentState);
        StateChanged?.Invoke(newState);
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
}
