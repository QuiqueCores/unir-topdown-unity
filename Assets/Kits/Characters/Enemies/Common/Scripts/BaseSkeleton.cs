using UnityEngine;

public class BaseSkeleton : BaseEnemy
{
    protected override void ExecuteAI()
    {
        Transform target = sight.GetClosestTarget();
        if (target != null)
        {
            Move((target.position - transform.position).normalized);
        }
    }
}
