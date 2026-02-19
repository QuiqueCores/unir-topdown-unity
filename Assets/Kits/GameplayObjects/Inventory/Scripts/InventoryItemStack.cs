using System;
using UnityEngine;

[Serializable]
public struct InventoryItemStack
{
    [SerializeField] private string itemId;
    [SerializeField] private int quantity;

    public string ItemId => itemId;
    public int Quantity => quantity;

    public InventoryItemStack(string itemId, int quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
    }
}