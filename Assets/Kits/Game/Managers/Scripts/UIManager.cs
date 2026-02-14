using UnityEngine;

public class UIManager : PersistentSingleton<UIManager>
{
    [SerializeField] private GameObject hudRootPrefab;

    private GameObject hudInstance;

    protected override void Awake()
    {
        base.Awake();
        EnsureHud();
    }

    private void EnsureHud()
    {
        if (hudInstance != null)
        {
            return;
        }

        hudInstance = Instantiate(hudRootPrefab);
        DontDestroyOnLoad(hudInstance);
    }
}