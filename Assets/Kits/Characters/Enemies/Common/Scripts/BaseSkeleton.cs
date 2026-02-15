using UnityEngine;
using System.Collections;

public class BaseSkeleton : BaseCharacter, IAttacker
{
    Sight2D sight;
    //protected float distancePlayer;
    //[SerializeField] float blinkInterval = 0.2f;
    MeleeAttack melee;

    [Header("Attack")]
    [SerializeField] int damage = 1;
    [SerializeField] float attackRange = 0.8f;

    public int Damage => damage;


    protected override void Awake()
    {
        base.Awake();
        sight = GetComponent<Sight2D>();
        melee = GetComponent<MeleeAttack>();
    }

    protected override void Update()
    {
        base.Update();

        Transform target = sight.GetClosestTarget();
        if (target == null)
            return;

        //Move((closestTarget.position - transform.position).normalized);
        //distancePlayer = transform.position - closestTarget.position;
        Vector2 dir = target.position - transform.position;
        Move(dir.normalized);

        if (dir.magnitude <= attackRange)
        {
            melee.TryAttack(dir.normalized);
        }

    }
  
}
