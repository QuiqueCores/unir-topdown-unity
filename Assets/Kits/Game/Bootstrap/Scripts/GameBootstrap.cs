using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [Header("Manager Prefabs")]
    [SerializeField] private GameObject managersRootPrefab;

    private void Awake()
    {
        if (managersRootPrefab != null)
        {
            EnsureManagersFromPrefab();
            return;
        }
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