using UnityEngine;
using System.Collections.Generic;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [SerializeField] private List<QuestStatus> activeQuests = new List<QuestStatus>();
    public List<QuestStatus> ActiveQuests => activeQuests;

    public static event Action OnQuestLogUpdated;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AcceptQuest(QuestDefinition quest)
    {
        if (activeQuests.Exists(q => q.QuestData == quest)) return;

        QuestStatus newStatus = new QuestStatus(quest);
        activeQuests.Add(newStatus);

        Debug.Log($"<color=green>Quest Accepted: {quest.questName}</color>");

        OnQuestLogUpdated?.Invoke();
    }

    private void OnEnable()
    {
        InventorySystem.OnItemAddedStatic += HandleItemAdded;
    }

    private void OnDisable()
    {
        InventorySystem.OnItemAddedStatic -= HandleItemAdded;
    }

    private void HandleItemAdded(string itemId, int amount)
    {
        foreach (QuestStatus status in activeQuests)
        {
            if (status.isCompleted) continue;

            for (int i = 0; i < status.QuestData.objectives.Count; i++)
            {
                if (status.QuestData.objectives[i] is CollectionObjective colObj)
                {
                    if (colObj.itemToCollect.ItemId == itemId)
                    {
                        status.currentAmounts[i] += amount;
                        CheckCompletion(status);
                    }
                }
            }
        }
        OnQuestLogUpdated?.Invoke();
    }

    private void CheckCompletion(QuestStatus status)
    {
        bool allDone = true;
        for (int i = 0; i < status.QuestData.objectives.Count; i++)
        {
            if (status.currentAmounts[i] < status.QuestData.objectives[i].requiredAmount)
            {
                allDone = false;
                break;
            }
        }
        status.isCompleted = allDone;
        if (allDone) Debug.Log($"<color=blue>¡Quest completed!</color>");
    }
}