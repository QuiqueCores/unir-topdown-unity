public class BaseSkeleton : BaseEnemy
{
    protected override void ExecuteAI()
    {
        base.ExecuteAI();
        Move(distance.normalized);
    }


}
