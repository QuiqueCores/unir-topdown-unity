using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemDefinitionSO item;
    [SerializeField] private int amount = 1;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        var inventory = collision.GetComponentInChildren<InventorySystem>();

        if (inventory != null)
        {
            inventory = collision.GetComponent<InventorySystem>();
        }

        if (inventory == null)
        {
            Debug.LogWarning("Player has no InventorySystem component.");
            return;
        }

        if (inventory.Add(item, amount))
        {
            Destroy(gameObject);
        }
    }
}