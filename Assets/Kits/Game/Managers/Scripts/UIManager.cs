using UnityEngine;

public class UIManager : PersistentSingleton<UIManager>
{
    [SerializeField] private GameObject UIRootPrefab;

    private GameObject UIInstance;

    protected override void Awake()
    {
        base.Awake();
        EnsureUI();
    }

    private void EnsureUI()
    {
        if (UIInstance != null)
        {
            return;
        }

        UIInstance = Instantiate(UIRootPrefab);
        DontDestroyOnLoad(UIInstance);
    }
}