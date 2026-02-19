using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;
    [Header("Manager Prefabs")]
    [SerializeField] private GameObject managersRootPrefab;

    private void Awake()
    {
        if (playerPrefab != null)
        {
            EnsurePlayerFromPrefab();
        }

        if (managersRootPrefab != null)
        {
            EnsureManagersFromPrefab();
        }
    }

    private void EnsurePlayerFromPrefab()
    {
        if (PlayerPersistent.Instance != null)
        {
            return;
        }

        Instantiate(playerPrefab);
    }

    private void EnsureManagersFromPrefab()
    {
        if (GameManager.Instance != null)
        {
            return;
        }

        Instantiate(managersRootPrefab);
    }
}