using System;

public class GameManager : PersistentSingleton<GameManager>
{
    public GameState State { get; private set; } = GameState.Playing;

    public event Action<GameState> OnStateChanged;

    public void SetState(GameState newState)
    {
        if (State == newState)
        {
            return;
        }
        State = newState;
        OnStateChanged?.Invoke(State);
    }
}
