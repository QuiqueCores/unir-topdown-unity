using UnityEngine;

public class Sight2D : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float checkFrequency = 5f;
    [Space]
    [SerializeField] IVisible2D.Side[] detectableSides;

    Transform closestTarget;
    float distanceToClosestTarget;
    int priorityOfClosestTarget;

    float lastCheckTime;
    Collider2D[] colliders;

    private void Update()
    {
        if ((Time.time - lastCheckTime) > (1f / checkFrequency))
        {
            lastCheckTime = Time.time;

            colliders = Physics2D.OverlapCircleAll(transform.position, radius);

            closestTarget = null;
            distanceToClosestTarget = Mathf.Infinity;
            priorityOfClosestTarget = -1;
            for (int i = 0; i < colliders.Length; i++)
            {
                IVisible2D visible = colliders[i].GetComponent<IVisible2D>();
                if ((visible != null) && CanSee(visible))
                {
                    float distanceToTarget = Vector3.Distance(transform.position, colliders[i].transform.position);
                    if (
                            (visible.GetPriority() > priorityOfClosestTarget) ||
                            ((visible.GetPriority() == priorityOfClosestTarget) && (distanceToTarget < distanceToClosestTarget))
                        )
                    {
                        closestTarget = colliders[i].transform;
                        distanceToClosestTarget = distanceToTarget;
                        priorityOfClosestTarget = visible.GetPriority();
                    }
                }
            }
        }
    }

    private bool CanSee(IVisible2D visible)
    {
        bool canSee = false;

        for (int i = 0; !canSee && (i < detectableSides.Length); i++)
        {
            canSee = visible.GetSide() == detectableSides[i];
        }

        return canSee;
    }

    public Transform GetClosestTarget()
    {
        return closestTarget;
    }

}
