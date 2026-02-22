using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] AudioClip attackSound;

    MeleeAttack melee;
    Vector2 rawMove;
    Vector2 punchDirection = Vector2.down;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction togglePauseAction;

    bool isAttacking = false;
    [SerializeField] float attackAnimationTime = 0.3f;

    [SerializeField] float interactRadius = 1.2f;
    [SerializeField] LayerMask interactableLayer;


    private bool isInteracting = false;
    public bool IsInteracting { get => isInteracting; set => isInteracting = value; }
    private bool subscribed;

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
        togglePauseAction = actions.FindAction("TogglePause", true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        TrySubscribeToGameManager();

        moveAction.Enable();

        moveAction.started += OnMove;
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        attackAction.Enable();
        attackAction.performed += OnPunch;

        togglePauseAction.Enable();
        togglePauseAction.performed += OnTogglePause;
    }

    private void Start()
    {
        TrySubscribeToGameManager();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleState;
        }
        subscribed = false;

        moveAction.Disable();

        moveAction.started -= OnMove;
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        attackAction.Disable();
        attackAction.performed -= OnPunch;

        togglePauseAction.Disable();
        togglePauseAction.performed -= OnTogglePause;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindCamera();
        TrySubscribeToGameManager();
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

        // Pasar direcci�n al animator
        animator.SetFloat("DireccionX", lastLookDirection.x);
        animator.SetFloat("DireccionY", lastLookDirection.y);

        animator.SetBool("Attack", true);

        // Ejecutar da�o
        melee.TryAttack(lastLookDirection);
        audioSource.PlayOneShot(attackSound);

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

        SceneTransitionManager.Instance.PlacePlayerAtSpawn("Spawn");

        if (rb2D != null)
            rb2D.linearVelocity = Vector2.zero;
    }

    private void OnTogglePause(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.State == GameState.Playing)
        {
            GameManager.Instance.SetState(GameState.Paused);
        }
        else if (GameManager.Instance.State == GameState.Paused)
        {
            GameManager.Instance.SetState(GameState.Playing);
        }
    }

    private void OnInteractPressed(InputAction.CallbackContext context)
    {
        if (isAttacking) return;

        TryInteract();
    }

    void TryInteract()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            interactRadius,
            interactableLayer
        );

        BaseInteractable best = null;
        float bestScore = -Mathf.Infinity;

        foreach (var hit in hits)
        {
            BaseInteractable interactable = hit.GetComponent<BaseInteractable>();
            if (interactable == null)
                continue;

            Vector2 toTarget = (hit.transform.position - transform.position).normalized;
            float score = Vector2.Dot(lastLookDirection, toTarget);

            if (score > bestScore)
            {
                bestScore = score;
                best = interactable;
            }
        }

        if (best != null && bestScore > 0.5f)
        {
            best.Interact(gameObject);
        }
    }

    private void HandleState(GameState state)
    {
        isInteracting = (state == GameState.Dialogue);
    }

    private void TrySubscribeToGameManager()
    {
        if (subscribed)
        {
            return;
        }
        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.OnStateChanged += HandleState;
        subscribed = true;

        HandleState(GameManager.Instance.State);
    }
}
