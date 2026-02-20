using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Item Database")]
    [SerializeField] private List<ItemDefinitionSO> itemDatabase = new();

    [Header("Initial Inventory")]
    [SerializeField] private List<InventoryItemStack> initialItems = new();

    // Runtime storage
    private readonly Dictionary<string, int> itemCounts = new();
    private readonly Dictionary<string, ItemDefinitionSO> itemDefinitions = new();

    public static event Action<string, int> OnItemAddedStatic;

    public event Action OnInventoryChanged;

    private void Awake()
    {
        BuildDatabase();
        LoadInitialItems();
    }

    private void BuildDatabase()
    {
        itemDefinitions.Clear();

        foreach (ItemDefinitionSO def in itemDatabase)
        {
            if (itemDefinitions.ContainsKey(def.ItemId))
            {
                Debug.LogError($"Duplicate ItemId '{def.ItemId}' in itemDatabase.", def);
                continue;
            }

            itemDefinitions.Add(def.ItemId, def);
        }
    }

    private void LoadInitialItems()
    {
        itemCounts.Clear();

        foreach (InventoryItemStack entry in initialItems)
        {
            AddById(entry.ItemId, entry.Quantity, silent: true);
        }

        OnInventoryChanged?.Invoke();
    }

    public bool TryGetDefinition(string itemId, out ItemDefinitionSO def)
    {
        return itemDefinitions.TryGetValue(itemId, out def);
    }

    public int GetCount(string itemId)
    {
        if (itemCounts.TryGetValue(itemId, out int qty))
        {
            return qty;
        }

        return 0;
    }

    public bool Add(ItemDefinitionSO def, int amount)
    {
        return AddById(def.ItemId, amount);
    }

    public bool AddById(string itemId, int amount, bool silent = false)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (!itemDefinitions.ContainsKey(itemId))
        {
            Debug.LogWarning($"Trying to add unknown itemId '{itemId}'. Add it to itemDatabase first.");
            return false;
        }

        int current = GetCount(itemId);
        itemCounts[itemId] = current + amount;

        if (!silent)
        {
            OnInventoryChanged?.Invoke();
            OnItemAddedStatic?.Invoke(itemId, amount);
        }
        return true;
    }

    public bool RemoveById(string itemId, int amount)
    {
        if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
        {
            return false;
        }

        int current = GetCount(itemId);
        if (current < amount)
        {
            return false;
        }

        int next = current - amount;
        if (next == 0)
        {
            itemCounts.Remove(itemId);
        }
        else
        {
            itemCounts[itemId] = next;
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    public IReadOnlyList<InventoryItemStack> ToList()
    {
        var list = new List<InventoryItemStack>(itemCounts.Count);

        foreach (var kv in itemCounts)
        {
            list.Add(new InventoryItemStack(kv.Key, kv.Value));
        }

        return list;
    }

    public IReadOnlyList<ItemDefinitionSO> GetKnownItems()
    {
        var result = new List<ItemDefinitionSO>(itemDefinitions.Count);

        foreach (var def in itemDefinitions.Values)
        {
            result.Add(def);
        }

        return result;
    }
}