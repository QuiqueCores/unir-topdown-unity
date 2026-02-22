using System;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    public GameState State { get; private set; } = GameState.MainMenu;

    public event Action<GameState> OnStateChanged;

    public void SetState(GameState newState)
    {
        if (State == newState)
        {
            return;
        }
        State = newState;

        switch (State)
        {
            case GameState.MainMenu:
            case GameState.Playing:
            case GameState.Dialogue:
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
            case GameState.Transition:
            case GameState.Victory:
            case GameState.Defeat:
                Time.timeScale = 0f;
                break;
        }

        OnStateChanged?.Invoke(State);
    }
}
