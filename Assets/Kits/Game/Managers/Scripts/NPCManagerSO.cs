using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NPCManagerSO", menuName = "ScriptableObjects/NPCManagerSO", order = 1)]
public class NPCManagerSO : ScriptableObject
{
    private PlayerCharacter player;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        player = FindFirstObjectByType<PlayerCharacter>();
    }

    public void ChangePlayerState(bool state)
    {
        player.IsInteracting = state;
    }
}
