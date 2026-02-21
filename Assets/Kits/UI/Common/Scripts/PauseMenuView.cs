using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuView : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.SetState(GameState.Playing);
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.SetState(GameState.MainMenu);
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}