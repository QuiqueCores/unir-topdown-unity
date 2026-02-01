using UnityEngine;

public class Sight2D : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float checkFrequency = 5f;
    [SerializeField] string targetTag = "Player";

    Transform closestTarget;
    float distanceToClosestTarget;

    float lastCheckTime;
    Collider2D[] colliders;

    private void Update()
    {
        if ((Time.time - lastCheckTime) > (1f / checkFrequency))
        {
            lastCheckTime = Time.time;

            Debug.Log("Checking sight");
            colliders = Physics2D.OverlapCircleAll(transform.position, radius);

            closestTarget = null;
            distanceToClosestTarget = Mathf.Infinity;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag(targetTag))
                {
                    float distanceToTarget = Vector3.Distance(transform.position, colliders[i].transform.position);
                    if (distanceToTarget < distanceToClosestTarget)
                    {
                        closestTarget = colliders[i].transform;
                        distanceToClosestTarget = distanceToTarget;
                    }
                }
            }
        }
    }

    public Transform GetClosestTarget()
    {
        return closestTarget;
    }

}
