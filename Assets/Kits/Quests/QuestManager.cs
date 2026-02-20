using UnityEngine;
using System.Collections.Generic;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<QuestStatus> activeQuests = new List<QuestStatus>();

    public static event Action OnQuestProgressUpdated;

    private void Awake() { /* Singleton logic */ }

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
                        Debug.Log($"Progress: {status.currentAmounts[i]}/{colObj.requiredAmount}");
                    }
                }
            }
            CheckCompletion(status);
        }
        OnQuestProgressUpdated?.Invoke();
    }

    private void CheckCompletion(QuestStatus status)
    {
        bool allObjectivesDone = true;
        for (int i = 0; i < status.QuestData.objectives.Count; i++)
        {
            if (status.currentAmounts[i] < status.QuestData.objectives[i].requiredAmount)
            {
                allObjectivesDone = false;
                break;
            }
        }

        if (allObjectivesDone)
        {
            status.isCompleted = true;
            Debug.Log($"<color=green>Misión Completada: {status.QuestData.questName}</color>");
        }
    }
}