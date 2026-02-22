using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitionManager : PersistentSingleton<SceneTransitionManager>
{
    [Header("Fade")]
    [SerializeField] private ScreenFader screenFaderPrefab;
    [SerializeField] private float fadeOutDuration = 0.25f;
    [SerializeField] private float fadeInDuration = 0.25f;

    private ScreenFader fader;
    private bool isTransitioning;

    protected override void Awake()
    {
        base.Awake();
        EnsureFaderExists();
    }

    public void RequestTransition(string sceneName, string spawnId, GameState targetState)
    {
        if (isTransitioning)
        {
            return;
        }

        StartCoroutine(TransitionRoutine(sceneName, spawnId, targetState));
    }

    private void EnsureFaderExists()
    {
        if (fader != null)
        {
            return;
        }

        fader = Instantiate(screenFaderPrefab);
        DontDestroyOnLoad(fader.gameObject);
    }

    private IEnumerator TransitionRoutine(string sceneName, string spawnId, GameState targetState)
    {
        isTransitioning = true;

        // Block gameplay
        GameManager.Instance.SetState(GameState.Transition);

        yield return null; // Allow state to propagate

        yield return fader.FadeOut(fadeOutDuration);

        // Load scene
        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!op.isDone)
        {
            yield return null;
        }

        // Reposition player
        PlacePlayerAtSpawn(spawnId);

        yield return fader.FadeIn(fadeInDuration);

        GameManager.Instance.SetState(targetState);

        isTransitioning = false;
    }

    public void PlacePlayerAtSpawn(string spawnId)
    {
        var spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        SpawnPoint match = null;

        foreach (var spawn in spawns)
        {
            if (spawn.SpawnId == spawnId)
            {
                match = spawn;
                break;
            }
        }

        if (match == null)
        {
            Debug.LogWarning($"SpawnPoint not found: '{spawnId}' in scene '{SceneManager.GetActiveScene().name}'.");
            return;
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found (tag 'Player'). Cannot place at spawn.");
            return;
        }

        player.transform.position = match.transform.position;
    }
}