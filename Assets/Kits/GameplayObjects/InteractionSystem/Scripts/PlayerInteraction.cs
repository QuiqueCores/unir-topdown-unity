using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float range = 1.2f;
    [SerializeField] InputActionReference interact;

    private void OnEnable()
    {
        interact.action.Enable();
        interact.action.performed += OnInteractTriggered;
    }

    private void OnDisable()
    {
        interact.action.performed -= OnInteractTriggered;
        interact.action.Disable();
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