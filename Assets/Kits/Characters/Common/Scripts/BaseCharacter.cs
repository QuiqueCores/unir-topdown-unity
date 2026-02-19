using UnityEngine;

public class BaseCharacter : MonoBehaviour, IVisible2D, IDamageable
{

    [SerializeField] float linearSpeed = 1f;

    [SerializeField] int priority = 0;
    [SerializeField] IVisible2D.Side side;

    Rigidbody2D rb2D;
    protected Animator animator;

    [Header("Lives")]
    [SerializeField] protected int maxLives = 1;
    protected int currentLives;
    bool isDead = false;

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentLives = maxLives;
        isDead = false;
    }

    protected virtual void Update()
    {
        animator.SetFloat("HorizontalVelocity", lastMoveDirection.x);
        animator.SetFloat("VerticalVelocity", lastMoveDirection.y);
    }

    Vector2 lastMoveDirection;

    public Transform Transform => transform;

    protected void Move(Vector2 direction)
    {
        rb2D.position += direction * linearSpeed * Time.deltaTime;
        lastMoveDirection = direction;
    }

    int IVisible2D.GetPriority()
    {
        return priority;
    }

    IVisible2D.Side IVisible2D.GetSide()
    {
        return side;
    }

    public virtual void TakeDamage(int amount)
    {


        if (isDead)
            return;

        currentLives -= amount;

        if (currentLives <= 0)
            Die();
    }
    protected void Die()
    {
        isDead = true;
        Destroy(gameObject);

    }
}
