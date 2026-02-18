using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public static InventoryUI instance;

    [Header("Debug Inventory")]
    public List<ItemDefinitionSO> currentItems = new List<ItemDefinitionSO>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public bool Contains(ItemDefinitionSO item)
    {
        if (item == null) return true;
        return currentItems.Contains(item);
    }

    public void AddItem(ItemDefinitionSO item)
    {
        if (!currentItems.Contains(item))
            currentItems.Add(item);
    }
}