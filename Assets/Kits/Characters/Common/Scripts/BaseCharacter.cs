using System.Collections;
using UnityEngine;
using static IVisible2D;

public class BaseCharacter : MonoBehaviour, IVisible2D, IDamageable
{

    [SerializeField] float linearSpeed = 1f;

    [SerializeField] int priority = 0;
    [SerializeField] IVisible2D.Side side;
    public void SetSide(Side side)
    {
        this.side = side;
    }

    protected Rigidbody2D rb2D;
    protected Animator animator;

    [SerializeField] protected AudioSource audioSource;

    [Header("Lives")]
    [SerializeField] protected int maxLives = 1;
    protected int currentLives;
    bool isDead = false;


    [Header("Damage Feedback")]
    [SerializeField] float flashDuration = 0.1f;
    [SerializeField] int flashCount = 2;

    SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentLives = maxLives;
        isDead = false;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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

        StartCoroutine(FlashRoutine());

        if (currentLives <= 0)
            Die();
    }
    protected virtual void Die()
    {
        isDead = true;
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    IEnumerator FlashRoutine()
    {
        if (spriteRenderer == null)
            yield break;

        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < flashCount; i++)
        {

            spriteRenderer.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                0.4f // alpha
            );

            yield return new WaitForSeconds(flashDuration);


            spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(flashDuration);
        }
    }


}
