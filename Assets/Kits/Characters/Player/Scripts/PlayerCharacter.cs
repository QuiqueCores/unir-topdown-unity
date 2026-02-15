using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BaseCharacter, IAttacker
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference punch;

    [Header("Punch")]
    //[SerializeField] float punchRadius = 0.3f;
    [SerializeField] float punchRange = 1.0f;

    //IAttacker
    [Header("Attack")]
    [SerializeField] int damage = 1;

    MeleeAttack melee;
    Vector2 rawMove;
    Vector2 punchDirection = Vector2.down;

    protected override void Awake()
    {
        base.Awake();
        melee = GetComponent<MeleeAttack>();
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

    private void OnDisable()
    {
        move.action.Disable();

        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;

        punch.action.Disable();
        punch.action.performed -= OnPunch;
    }

    protected override void Update()
    {
        base.Update();

        Move(rawMove);

        //if (mustPunch)
        //{
        //    mustPunch = false;
        //    PerformPunch();
        //}
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.action.ReadValue<Vector2>();

        if (rawMove.magnitude > 0f)
        {
            punchDirection = rawMove.normalized;
        }
    }

    //IAttacker
    public int Damage => damage;
    private void OnPunch(InputAction.CallbackContext context)
    {
        melee.TryAttack(punchDirection);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, punchDirection * punchRange);
    }
}
