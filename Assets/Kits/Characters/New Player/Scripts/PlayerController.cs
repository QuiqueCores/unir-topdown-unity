using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseCharacter
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference punch;

    //Animator animator;


    [Header("Punch")]
    [SerializeField] float punchRadius = 0.3f;
    [SerializeField] float punchRange = 1.0f;

    protected override void Awake()
    {
        base.Awake();
        cam = Camera.main;
        //animator = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        move.action.Enable();

        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;

        punch.action.Enable();
        punch.action.performed += OnPunch;
    }

    protected override void Update()
    {
        base.Update();

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



        if (mustPunch)
        {
            mustPunch = false;
            PerformPunch();
        }
    }

    private void OnDisable()
    {
        move.action.Disable();

        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;

        punch.action.Disable();
        punch.action.performed -= OnPunch;
    }

    Vector2 rawMove;
    private void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.action.ReadValue<Vector2>();

        if (rawMove.magnitude > 0f)
        {
            punchDirection = rawMove.normalized;
        }
    }

    bool mustPunch;
    private void OnPunch(InputAction.CallbackContext context)
    {
        mustPunch = true;
    }

    Vector2 punchDirection = Vector2.down;
    private void PerformPunch()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, punchRadius, punchDirection * punchRange);

        foreach (RaycastHit2D hit in hits)
        {
            BaseCharacter otherBaseCharacter = hit.collider.GetComponent<BaseCharacter>();
            if (otherBaseCharacter != this)
            {
                //otherBaseCharacter?.NotifyPunch();
            }
        }
    }

    public Vector2 lookDirection
    {
        get; private set;
    }
    Vector2 lastLookDirection = Vector2.down;
    Camera cam;

    Vector2 GetMouseLookDirection()
    {
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
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = (mouseWorld - transform.position);

        if (dir.sqrMagnitude < 0.01f)
        {
            return punchDirection;
        }
        return dir.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, punchDirection * punchRange);
    }
}
