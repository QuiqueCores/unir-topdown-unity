using System;

[Serializable]
public class QuestStatus
{
    private QuestDefinition _questData;
    public QuestDefinition QuestData => _questData;

    public bool isCompleted;
    public int[] currentAmounts;

    public QuestStatus(QuestDefinition definition)
    {
        _questData = definition;
        isCompleted = false;
        currentAmounts = new int[definition.objectives.Count];
    }
}