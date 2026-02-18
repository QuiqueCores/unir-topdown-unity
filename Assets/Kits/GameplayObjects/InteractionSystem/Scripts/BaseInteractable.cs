using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [Header("Requirements")]
    [SerializeField] protected ItemDefinitionSO requiredItem;

    public void Interact(GameObject requester)
    {
        if (CheckCondition(requester))
        {
            OnInteract(requester);
        }
        else
        {
            Debug.Log($"Lacks object: {requiredItem.ItemId}");
        }
    }

    protected virtual bool CheckCondition(GameObject requester)
    {
        if (requiredItem == null) return true;

        var inventory = requester.GetComponentInChildren<InventorySystem>();

        if (inventory == null)
        {
            inventory = requester.GetComponent<InventorySystem>();
        }

        if (inventory == null)
        {
            Debug.LogWarning("Requester has no InventorySystem.");
            return false;
        }

        return inventory.GetCount(requiredItem.ItemId) > 0;
    }

    protected abstract void OnInteract(GameObject requester);
}
