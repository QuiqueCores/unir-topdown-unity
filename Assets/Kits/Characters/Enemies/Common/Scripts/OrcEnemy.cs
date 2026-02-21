using UnityEngine;

public class OrcEnemy : BaseEnemy
{
    protected override void ExecuteAI()
    {
        base.ExecuteAI();
        Move(distance.normalized);
    }
}
