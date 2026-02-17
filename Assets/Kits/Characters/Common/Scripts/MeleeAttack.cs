using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] float attackRadius = 0.5f;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] float attackRange = 1.0f;

    IAttacker attacker;
    bool canAttack = true;

    private void Awake()
    {
        attacker = GetComponent<IAttacker>();
    }
    public void TryAttack(Vector2 direction)
    {
        if (!canAttack || attacker == null)
            return;

        StartCoroutine(AttackRoutine(direction));
    }

    IEnumerator AttackRoutine(Vector2 direction)
    {
        canAttack = false;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            attackRadius,
            direction,
            attackRange
        );

        foreach (RaycastHit2D hit in hits)
        {
            BaseCharacter target = hit.collider.GetComponent<BaseCharacter>();
            if (target != null && target.gameObject != gameObject)
            {
                target.TakeDamage(attacker.Damage);
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

}
