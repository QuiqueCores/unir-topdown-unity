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
                promptChannel.Raise("Press E to interact");
                return;
            }
        }

        promptChannel.Raise("");
    }


    private void OnInteractTriggered(InputAction.CallbackContext context)
    {
        PerformInteraction();
    }

    //private void PerformInteraction()
    //{
    //    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
    //
    //    foreach (Collider2D hit in hits)
    //    {
    //        IInteractable interactable = hit.GetComponent<IInteractable>();
    //
    //        if (interactable != null && hit.gameObject != gameObject)
    //        {
    //            Debug.Log($"<color=green>Interface detectada en:</color> {hit.gameObject.name}");
    //            interactable.Interact(this.gameObject);
    //            return;
    //        }
    //    }
    //
    //    Debug.Log("No IInteractable found nearby.");
    //}

    private void PerformInteraction()
    {
        PlayerCharacter player = GetComponent<PlayerCharacter>();
        Vector2 dir = player.lookDirection;

        Debug.Log("==== INTERACT PRESSED ====");

        if (dir == Vector2.zero)
        {
            Debug.Log("Dirección cero → CANCELADO");
            return;
        }

        Vector2 targetCell = (Vector2)transform.position + dir;

        Debug.Log("Celda objetivo: " + targetCell);

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            targetCell,
            Vector2.one * 0.9f,
            0f
        );

        Debug.Log("Colliders detectados: " + hits.Length);

        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable == null)
                continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestInteractable = interactable;
            }
        }

        if (closestInteractable != null)
        {
            Debug.Log("Interactuando con: " + ((MonoBehaviour)closestInteractable).name);
            closestInteractable.Interact(gameObject);
        }
        else
        {
            Debug.Log("No se encontró IInteractable válido");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}