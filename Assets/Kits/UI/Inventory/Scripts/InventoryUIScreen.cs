using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUIScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject screenRoot;
    [SerializeField] private Transform gridParent;
    [SerializeField] private InventorySlotView slotPrefab;

    [Header("Item Details")]
    [SerializeField] private Image detailsImage;
    [SerializeField] private TMP_Text detailsNameText;
    [SerializeField] private TMP_Text detailsDescriptionText;

    [Header("Input")]
    [SerializeField] private float toggleCooldown = 0.15f;

    private InventorySystem inventory;
    private readonly List<InventorySlotView> spawnedSlots = new();
    private bool isOpen;
    private PlayerInput playerInput;
    private InputAction toggleInventoryAction;
    private float nextToggleTime;

    private void Awake()
    {
        screenRoot.SetActive(false);
        inventory = PlayerPersistent.Instance.Character.GetComponent<InventorySystem>();
        playerInput = PlayerPersistent.Instance.Character.GetComponent<PlayerInput>();
        toggleInventoryAction = playerInput.actions.FindAction("ToggleInventory", true);
    }

    private void OnEnable()
    {
        inventory.OnInventoryChanged += Refresh;

        toggleInventoryAction.performed += OnTogglePerformed;
        toggleInventoryAction.Enable();
    }

    private void OnDisable()
    {
        inventory.OnInventoryChanged -= Refresh;

        toggleInventoryAction.performed -= OnTogglePerformed;
        toggleInventoryAction.Disable();
    }

    private void Start()
    {
        Refresh();
    }

    private void OnTogglePerformed(InputAction.CallbackContext ctx)
    {
        Toggle();
    }

    public void Toggle()
    {
        if (Time.unscaledTime < nextToggleTime)
        {
            return;
        }
        nextToggleTime = Time.unscaledTime + toggleCooldown;

        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        isOpen = true;
        screenRoot.SetActive(true);

        playerInput.SwitchCurrentActionMap("UI");
        RebindToggleAction();

        Refresh();

        // Select first slot
        if (spawnedSlots.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(spawnedSlots[0].gameObject);
        }
    }

    public void Close()
    {
        isOpen = false;
        screenRoot.SetActive(false);

        playerInput.SwitchCurrentActionMap("Player");
        RebindToggleAction();

        // Clear selection
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void RebindToggleAction()
    {
        toggleInventoryAction.performed -= OnTogglePerformed;
        toggleInventoryAction.Disable();

        toggleInventoryAction = playerInput.actions.FindAction("ToggleInventory", true);

        toggleInventoryAction.performed += OnTogglePerformed;
        toggleInventoryAction.Enable();
    }

    private void Refresh()
    {
        ClearSlots();

        IReadOnlyList<InventoryItemStack> items = inventory.ToList();
        foreach (var stack in items)
        {
            if (!inventory.TryGetDefinition(stack.ItemId, out var def))
                continue;

            var slot = Instantiate(slotPrefab, gridParent);
            slot.Bind(def.ItemId, def.Icon, stack.Quantity);
            slot.OnSelected += HandleSlotSelected;
            slot.OnClicked += HandleSlotClicked;

            spawnedSlots.Add(slot);
        }

        if (isOpen && spawnedSlots.Count == 0)
        {
            detailsImage.sprite = null;
            detailsImage.color = new Color(1f, 1f, 1f, 0f); ;
            detailsNameText.text = "Inventory is empty";
            detailsDescriptionText.text = "";
        }
    }

    private void ClearSlots()
    {
        for (int i = 0; i < spawnedSlots.Count; i++)
        {
            var s = spawnedSlots[i];
            if (s != null)
            {
                s.OnSelected -= HandleSlotSelected;
                s.OnClicked -= HandleSlotClicked;
                Destroy(s.gameObject);
            }
        }
        spawnedSlots.Clear();
    }

    private void HandleSlotSelected(InventorySlotView slot)
    {
        if (inventory.TryGetDefinition(slot.ItemId, out var def))
        {
            detailsImage.sprite = def.Icon;
            detailsImage.color = Color.white; ;
            detailsNameText.text = def.DisplayName;
            detailsDescriptionText.text = def.Description;
        }
    }

    private void HandleSlotClicked(InventorySlotView slot)
    {
        HandleSlotSelected(slot);

        // TODO: item use, ...
    }
}
