using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ScenePortal : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private string targetSpawnId;

    [Header("Behaviour")]
    [SerializeField] private bool requirePlayerTag = true;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (requirePlayerTag && !collision.CompareTag(playerTag))
        {
            return;
        }

        SceneTransitionManager.Instance.RequestTransition(targetSceneName, targetSpawnId);
    }
}