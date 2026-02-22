using UnityEngine;

public class MainMenuView : MonoBehaviour
{
    public void Play()
    {
        SceneTransitionManager.Instance.RequestTransition("Introduction", "Spawn", GameState.Playing);
    }

    public void Quit()
    {
        Application.Quit();
    }
}