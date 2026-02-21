using UnityEngine;
using UnityEngine.InputSystem;

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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            if (interactable != null && hit.gameObject != gameObject)
            {
                Debug.Log($"<color=green>Interface detectada en:</color> {hit.gameObject.name}");
                interactable.Interact(this.gameObject);
                return;
            }
        }

        Debug.Log("No IInteractable found nearby.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}