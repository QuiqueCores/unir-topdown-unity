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
        if (requiredItem == null) return true; // Sen condición

        // Chamada ao sistema que viches en clase
        return InventoryUI.instance.Contains(requiredItem);
    }

    // O que fai o obxecto ao activarse
    protected abstract void OnInteract(GameObject requester);
}
