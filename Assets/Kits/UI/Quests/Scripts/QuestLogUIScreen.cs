using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class QuestLogUIScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject screenRoot;
    [SerializeField] private Transform listParent;
    [SerializeField] private QuestListItemView rowPrefab;

    [Header("Details")]
    [SerializeField] private TMP_Text detailsTitleText;
    [SerializeField] private TMP_Text detailsStoryText;
    [SerializeField] private TMP_Text detailsObjectivesText;

    [Header("Input")]
    [SerializeField] private float toggleCooldown = 0.15f;

    private readonly List<QuestListItemView> spawned = new();
    private bool isOpen;

    private PlayerInput playerInput;
    private InputAction toggleAction;
    private float nextToggleTime;

    private void Awake()
    {
        screenRoot.SetActive(false);

        playerInput = PlayerPersistent.Instance.Character.GetComponent<PlayerInput>();
        toggleAction = playerInput.actions.FindAction("ToggleQuestLog", true);
    }

    private void OnEnable()
    {
        QuestManager.OnQuestLogUpdated += Refresh;

        toggleAction.performed += OnTogglePerformed;
        toggleAction.Enable();
    }

    private void OnDisable()
    {
        QuestManager.OnQuestLogUpdated -= Refresh;

        toggleAction.performed -= OnTogglePerformed;
        toggleAction.Disable();
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
        if (GameManager.Instance.State != GameState.Playing)
        {
            return;
        }

        isOpen = true;
        screenRoot.SetActive(true);

        // playerInput.SwitchCurrentActionMap("UI");
        // RebindToggleAction();
        GameManager.Instance.SetState(GameState.QuestLog);

        Refresh();

        if (spawned.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(spawned[0].gameObject);
        }
    }

    public void Close()
    {
        isOpen = false;
        screenRoot.SetActive(false);

        // playerInput.SwitchCurrentActionMap("Player");
        // RebindToggleAction();
        GameManager.Instance.SetState(GameState.Playing);

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void RebindToggleAction()
    {
        toggleAction.performed -= OnTogglePerformed;
        toggleAction.Disable();

        toggleAction = playerInput.actions.FindAction("ToggleQuestLog", true);

        toggleAction.performed += OnTogglePerformed;
        toggleAction.Enable();
    }

    private void Refresh()
    {
        ClearList();

        if (QuestManager.instance == null)
        {
            SetEmptyDetails("Quest Log", "QuestManager not found in scene.", "");
            return;
        }

        var active = QuestManager.instance.ActiveQuests;

        if (active == null || active.Count == 0)
        {
            SetEmptyDetails("No hay misiones activas", "Acepta una misi�n para que aparezca aqu�.", "");
            return;
        }

        foreach (var status in active)
        {
            var row = Instantiate(rowPrefab, listParent);
            row.Bind(status);
            row.OnSelected += HandleSelected;
            spawned.Add(row);
        }

        // Show first quest details
        if (isOpen && spawned.Count > 0)
            HandleSelected(spawned[0]);
    }

    private void ClearList()
    {
        foreach (var r in spawned)
        {
            if (r == null)
            {
                continue;
            }
            r.OnSelected -= HandleSelected;
            Destroy(r.gameObject);
        }
        spawned.Clear();
    }

    private void HandleSelected(QuestListItemView view)
    {
        var status = view.BoundStatus;

        detailsTitleText.text = status.QuestData.questName;
        detailsStoryText.text = status.QuestData.questStory;

        detailsObjectivesText.text = BuildObjectivesText(status);
    }

    private string BuildObjectivesText(QuestStatus status)
    {
        var def = status.QuestData;
        List<string> lines = new()
        {
            "<b>Objetivos:</b>"
        };

        if (def.objectives == null || def.objectives.Count == 0)
        {
            lines.Add("    No hay objetivos.");
        }

        for (int i = 0; i < def.objectives.Count; i++)
        {
            var obj = def.objectives[i];
            if (obj == null)
            {
                Debug.LogWarning($"Objective at index {i} is null in quest: {def.questName}");
                continue;
            }

            char objectiveBullet = '\u25AB';
            if (status.currentAmounts[i] == obj.requiredAmount)
            {
                objectiveBullet = '\u25AA';
            }
            lines.Add($"    {objectiveBullet} {obj.objectiveDescription}  ({status.currentAmounts[i]}/{obj.requiredAmount}).");
        }

        if (status.isCompleted)
        {
            lines.Add("    \u25AB Vuelve a hablar con quien te encomend� esta misi�n para recibir la recompensa.");
        }

        return string.Join("\n", lines);
    }

    private void SetEmptyDetails(string title, string story, string objectives)
    {
        detailsTitleText.text = title ?? "";
        detailsStoryText.text = story ?? "";
        detailsObjectivesText.text = objectives ?? "";
    }
}