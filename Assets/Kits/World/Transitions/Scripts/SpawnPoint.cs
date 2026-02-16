using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private string spawnId;

    public string SpawnId => spawnId;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
