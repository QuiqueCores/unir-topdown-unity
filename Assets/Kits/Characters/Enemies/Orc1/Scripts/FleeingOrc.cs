public class FleeingOrc : BaseEnemy
{
    protected override void ExecuteAI()
    {
        base.ExecuteAI();
        Move(-distance.normalized);
    }
}
