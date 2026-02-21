using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestTrackerHUD : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform activeListParent;
    [SerializeField] private TMP_Text linePrefab;
    [SerializeField] private int maxLines = 3;

    private readonly List<TMP_Text> spawned = new();

    private void OnEnable()
    {
        QuestManager.OnQuestLogUpdated += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        QuestManager.OnQuestLogUpdated -= Refresh;
        Clear();
    }

    private void Refresh()
    {
        Clear();

        if (QuestManager.instance == null || QuestManager.instance.ActiveQuests == null)
        {
            return;
        }

        var active = QuestManager.instance.ActiveQuests;
        int count = Mathf.Min(maxLines, active.Count);

        if (count == 0)
        {
            var line = Instantiate(linePrefab, activeListParent);
            line.text = "No hay misiones activas";
            line.fontSize = 24;
            spawned.Add(line);
            return;
        }

        for (int i = 0; i < count; i++)
        {
            var qs = active[i];

            var line = Instantiate(linePrefab, activeListParent);
            line.text = BuildLine(qs);
            line.fontSize = 24;
            spawned.Add(line);
        }

        if (active.Count > count)
        {
            var line = Instantiate(linePrefab, activeListParent);
            line.text = $"... y {active.Count - count} misiones más.";
            line.fontSize = 22;
            spawned.Add(line);
        }

        var lastLine = Instantiate(linePrefab, activeListParent);
        lastLine.text = "Pulsa \"<b>Q</b>\" para abrir el diario de misiones.";
        lastLine.fontSize = 20;
        spawned.Add(lastLine);
    }

    private string BuildLine(QuestStatus qs)
    {
        string statusText = "[En curso] ";
        if (qs.isCompleted)
        {
            statusText = "[Completada] ";
        }
        return $"{statusText}{qs.QuestData.questName}";
    }

    private void Clear()
    {
        for (int i = 0; i < spawned.Count; i++)
        {
            if (spawned[i] != null)
            {
                Destroy(spawned[i].gameObject);
            }
        }
        spawned.Clear();
    }
}