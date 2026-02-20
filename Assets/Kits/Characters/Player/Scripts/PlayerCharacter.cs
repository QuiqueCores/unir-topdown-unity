using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerCharacter : BaseCharacter, IAttacker
{
    [SerializeField] private FloatEventChannelSO healthChannel;

    [Header("Punch")]
    //[SerializeField] float punchRadius = 0.3f;
    [SerializeField] float punchRange = 2.0f;

    //IAttacker
    [Header("Attack")]
    [SerializeField] int damage = 1;

    [Header("Respawn")]
    [SerializeField] Transform respawnPoint;


    MeleeAttack melee;
    Vector2 rawMove;
    Vector2 punchDirection = Vector2.down;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction attackAction;

    bool isAttacking = false;
    [SerializeField] float attackAnimationTime = 0.3f;


    private bool isInteracting = false;
    public bool IsInteracting { get => isInteracting; set => isInteracting = value; }

    protected override void Awake()
    {
        base.Awake();
        BindCamera();
        melee = GetComponent<MeleeAttack>();

        // Wire input actions
        playerInput = GetComponent<PlayerInput>();

        var actions = playerInput.actions;

        moveAction = actions.FindAction("Move", true);
        attackAction = actions.FindAction("Attack", true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        moveAction.Enable();

        moveAction.started += OnMove;
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        attackAction.Enable();
        attackAction.performed += OnPunch;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        moveAction.Disable();

        moveAction.started -= OnMove;
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        attackAction.Disable();
        attackAction.performed -= OnPunch;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindCamera();
    }

    protected override void Update()
    {
        base.Update();

        if (cam == null)
        {
            BindCamera();
            if (cam == null) return;
        }

        if (!isInteracting && !isAttacking)
        {
            Move(rawMove);

            bool isMoving = rawMove.sqrMagnitude > 0.01f;

            lookDirection = GetMouseLookDirection();
            punchDirection = GetPunchDirection();

            if (lookDirection != Vector2.zero)
            {
                lastLookDirection = lookDirection;
            }

            if (isMoving)
            {
                animator.SetFloat("HorizontalVelocity", lastLookDirection.x);
                animator.SetFloat("VerticalVelocity", lastLookDirection.y);

            }
            else
            {
                animator.SetFloat("HorizontalVelocity", 0);
                animator.SetFloat("VerticalVelocity", 0);
                animator.SetFloat("DireccionX", lastLookDirection.x);
                animator.SetFloat("DireccionY", lastLookDirection.y);

            }
        }
        //if (mustPunch)
        //{
        //    mustPunch = false;
        //    PerformPunch();
        //}
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.action.ReadValue<Vector2>();
    }

    //IAttacker
    public int Damage => damage;
    private void OnPunch(InputAction.CallbackContext context)
    {
        if (isInteracting || isAttacking)
            return;

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // Bloquear movimiento
        rawMove = Vector2.zero;

        // Pasar dirección al animator
        animator.SetFloat("DireccionX", lastLookDirection.x);
        animator.SetFloat("DireccionY", lastLookDirection.y);

        animator.SetBool("Attack", true);

        // Ejecutar daño
        melee.TryAttack(lastLookDirection);

        yield return new WaitForSeconds(attackAnimationTime);

        animator.SetBool("Attack", false);

        isAttacking = false;
    }


    //bool mustPunch;
    //private void OnPunch(InputAction.CallbackContext context)
    //{
    //    mustPunch = true;
    //}

    //private void PerformPunch()
    //{
    //
    //    RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, punchRadius, punchDirection * punchRange);
    //
    //    foreach (RaycastHit2D hit in hits)
    //    {
    //        BaseCharacter otherBaseCharacter = hit.collider.GetComponent<BaseCharacter>();
    //        if (otherBaseCharacter != this)
    //        {
    //            otherBaseCharacter?.NotifyPunch();
    //        }
    //    }
    //}

    public Vector2 lookDirection
    {
        get; private set;
    }


    Vector2 lastLookDirection = Vector2.down;
    Camera cam;

    public void BindCamera()
    {
        cam = Camera.main;
    }

    Vector2 GetMouseLookDirection()
    {
        if (cam == null) return lookDirection;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = (mouseWorld - transform.position);

        if (dir.sqrMagnitude < 0.01f)
        {
            return lookDirection;
        }
        dir.Normalize();

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            return new Vector2(Mathf.Sign(dir.x), 0f);
        }
        else
        {
            return new Vector2(0f, Mathf.Sign(dir.y));
        }
    }
    Vector2 GetPunchDirection()
    {
        if (cam == null) return punchDirection;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = (mouseWorld - transform.position);

        if (dir.sqrMagnitude < 0.01f)
        {
            return punchDirection;
        }
        return dir.normalized;
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        healthChannel.Raise((float)currentLives / maxLives);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, punchDirection * punchRange);
    }

    protected override void Die()
    {
        Respawn();
    }

    private void Respawn()
    {
        currentLives = maxLives;

        transform.position = respawnPoint.position;

        if (rb2D != null)
            rb2D.linearVelocity = Vector2.zero;
    }



}
