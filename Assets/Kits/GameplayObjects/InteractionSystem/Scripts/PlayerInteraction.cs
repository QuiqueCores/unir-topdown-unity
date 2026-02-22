using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float range = 1.2f;
    private PlayerInput playerInput;
    private InputAction interactAction;

    [SerializeField] private StringEventChannelSO promptChannel;

    private void Awake()
    {
        // Wire input actions
        playerInput = GetComponent<PlayerInput>();

        var actions = playerInput.actions;

        interactAction = actions.FindAction("Interact", true);
    }

    private void OnEnable()
    {
        interactAction.Enable();
        interactAction.performed += OnInteractTriggered;
    }

    private void OnDisable()
    {
        interactAction.performed -= OnInteractTriggered;
        interactAction.Disable();
    }

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            if (interactable != null && hit.gameObject != gameObject)
            {
                promptChannel.Raise("Pulsa \"<b>E</b>\" para interactuar");
                return;
            }
        }

        promptChannel.Raise("");
    }


    private void OnInteractTriggered(InputAction.CallbackContext context)
    {
        PerformInteraction();
    }

    private void PerformInteraction()
    {
        PlayerCharacter player = GetComponent<PlayerCharacter>();
        Vector2 dir = player.lookDirection;

        if (dir == Vector2.zero)
            return;

        Vector2 targetCell = (Vector2)transform.position + dir;

        Collider2D[] cellHits = Physics2D.OverlapBoxAll(
            targetCell,
            Vector2.one * 0.9f,
            0f
        );

        foreach (var hit in cellHits)
        {
            PushableObject rock = hit.GetComponent<PushableObject>();
            if (rock != null)
            {
                rock.Interact(gameObject);
                return;
            }
        }

        Collider2D[] nearby = Physics2D.OverlapCircleAll(
            transform.position,
            1.2f
        );

        foreach (var hit in nearby)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(gameObject);
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}