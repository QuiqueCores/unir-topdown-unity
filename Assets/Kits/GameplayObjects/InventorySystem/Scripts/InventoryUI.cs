using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    
    public static InventoryUI instance;

    [Header("Debug Inventory")]
    public List<InventoryItemDefinition> currentItems = new List<InventoryItemDefinition>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public bool Contains(InventoryItemDefinition item)
    {
        if (item == null) return true;
        return currentItems.Contains(item);
    }

    public void AddItem(InventoryItemDefinition item)
    {
        if (!currentItems.Contains(item))
            currentItems.Add(item);
    }
}