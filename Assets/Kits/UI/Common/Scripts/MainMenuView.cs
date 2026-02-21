using UnityEngine;

public class MainMenuView : MonoBehaviour
{
    public void Play()
    {
        GameManager.Instance.SetState(GameState.Playing);
        SceneTransitionManager.Instance.RequestTransition("Introduction", "Spawn");
    }

    public void Quit()
    {
        Application.Quit();
    }
}