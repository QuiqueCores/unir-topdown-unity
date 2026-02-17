using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [Header("Requirements")]
    [SerializeField] protected InventoryItemDefinition requiredItem;

    public void Interact(GameObject requester)
    {
        if (CheckCondition())
        {
            OnInteract(requester);
        }
        else
        {
            Debug.Log($"Falta o obxecto: {requiredItem.uniqueItemName}");
        }
    }

    protected virtual bool CheckCondition()
    {
        if (requiredItem == null) return true;

        return InventoryUI.instance.Contains(requiredItem);
    }

    protected abstract void OnInteract(GameObject requester);
}
