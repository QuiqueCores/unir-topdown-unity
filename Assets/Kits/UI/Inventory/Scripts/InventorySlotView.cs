using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour, ISelectHandler
{
    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Button button;

    private string itemId;
    public string ItemId => itemId;

    public event Action<InventorySlotView> OnSelected;
    public event Action<InventorySlotView> OnClicked;

    public void Bind(string newItemId, Sprite icon, int quantity)
    {
        itemId = newItemId;

        iconImage.sprite = icon;
        iconImage.enabled = icon != null;

        quantityText.text = "";
        if (quantity > 1)
        {
            quantityText.text = quantity.ToString();
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClicked?.Invoke(this));
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnSelected?.Invoke(this);
    }
}
