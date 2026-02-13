using UnityEngine;

public abstract class BaseEnemy : BaseCharacter
{
    protected Sight2D sight;

    protected override void Awake()
    {
        base.Awake();
        sight = GetComponent<Sight2D>();
    }

    protected override void Update()
    {
        base.Update();
        ExecuteAI();
    }

    protected abstract void ExecuteAI();
}