using UnityEngine;

public class FleeingOrc : BaseEnemy
{
    protected override void ExecuteAI()
    {
        Transform target = sight.GetClosestTarget();
        if (target != null)
        {
            Move((transform.position - target.position).normalized);
        }
    }
}
