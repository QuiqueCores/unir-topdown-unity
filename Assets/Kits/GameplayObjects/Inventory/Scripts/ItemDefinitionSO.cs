using UnityEngine;

[CreateAssetMenu(menuName = "Kits/Inventory/Item Definition", fileName = "Item_")]
public class ItemDefinitionSO : ScriptableObject
{
    [SerializeField] private string itemId;

    public string ItemId => itemId;
}