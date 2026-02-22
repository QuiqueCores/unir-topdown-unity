using UnityEngine;

public class VictoryMenuView : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneTransitionManager.Instance.RequestTransition("MainMenu", "Spawn", GameState.MainMenu);
    }

    public void Quit()
    {
        Application.Quit();
    }
}