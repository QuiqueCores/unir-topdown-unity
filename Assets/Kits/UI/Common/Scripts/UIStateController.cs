using UnityEngine;
using UnityEngine.EventSystems;

public class UIStateController : MonoBehaviour
{
    [Header("Roots")]
    [SerializeField] private GameObject hudRoot;
    [SerializeField] private GameObject pauseMenuRoot;
    [SerializeField] private GameObject mainMenuRoot;

    [Header("First Selected")]
    [SerializeField] private GameObject pauseFirstSelected;
    [SerializeField] private GameObject mainMenuFirstSelected;

    private void OnEnable()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
        HandleStateChanged(GameManager.Instance.State);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(GameState state)
    {
        hudRoot.SetActive(false);
        pauseMenuRoot.SetActive(false);
        mainMenuRoot.SetActive(false);

        switch (state)
        {
            case GameState.MainMenu:
                mainMenuRoot.SetActive(true);
                Select(mainMenuFirstSelected);
                break;

            case GameState.Dialogue:
            case GameState.QuestLog:
            case GameState.Inventory:
            case GameState.Playing:
                hudRoot.SetActive(true);
                break;

            case GameState.Paused:
                pauseMenuRoot.SetActive(true);
                Select(pauseFirstSelected);
                break;

            case GameState.Transition:
            case GameState.Victory:
            case GameState.Defeat:
                break;
        }
    }

    private void Select(GameObject go)
    {
        if (!go)
        {
            return;
        }
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(go);
        }
    }
}