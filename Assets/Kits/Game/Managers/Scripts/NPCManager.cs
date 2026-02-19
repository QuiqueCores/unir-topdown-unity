using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCManager : PersistentSingleton<NPCManager>
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
        //player.IsInteracting = state;
    }
}
