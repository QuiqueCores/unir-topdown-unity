using UnityEngine;
using System.Collections.Generic;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public List<QuestStatus> activeQuests = new List<QuestStatus>();

    public static event Action OnQuestLogUpdated;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AcceptQuest(QuestDefinition quest)
    {
        if (activeQuests.Exists(q => q.QuestData == quest)) return;

        activeQuests.Add(new QuestStatus(quest));
        Debug.Log($"Challenge accepted: {quest.questName}");

        OnQuestLogUpdated?.Invoke();
    }

    public void UpdateProgress(QuestObjective objective, int amount)
    {
        OnQuestLogUpdated?.Invoke();
    }
}