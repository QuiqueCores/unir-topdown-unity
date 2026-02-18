using UnityEngine;

[CreateAssetMenu(menuName = "Kits/Inventory/Item Definition", fileName = "Item_")]
public class ItemDefinitionSO : ScriptableObject
{
    [Header("Item Details")]
    [SerializeField] private string itemId;
    [SerializeField] private string displayName;

    [Header("Presentation")]
    [SerializeField] private Sprite icon;

    [Header("Stacking")]
    [SerializeField] private bool stackable = true;
    [SerializeField] private int maxStack = 99;

    public string ItemId => itemId;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public bool Stackable => stackable;
    public int MaxStack => Mathf.Max(1, maxStack);
}