using JetBrains.Annotations;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class BaseEnemy : BaseCharacter, IAttacker
{
    protected Sight2D sight;
    MeleeAttack melee;

    [Header("Attack")]
    [SerializeField] public int damage = 1;
    [SerializeField] public float attackRange = 0.8f;

    protected override void Awake()
    {
        base.Awake();
        sight = GetComponent<Sight2D>();
        melee = GetComponent<MeleeAttack>();
    }

    public int Damage => damage;


    protected override void Update()
    {
        base.Update();
        ExecuteAI();
    }

    public Vector2 distance;
    protected virtual void ExecuteAI()
    {
        Transform target = sight.GetClosestTarget();
        if (target == null)
            return;

        distance = (target.position - transform.position);

        if (distance.magnitude <= attackRange)
        {
            melee.TryAttack(distance.normalized);
        }
    }
}