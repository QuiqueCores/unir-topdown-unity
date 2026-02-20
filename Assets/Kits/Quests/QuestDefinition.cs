using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest Definition")]
public class QuestDefinition : ScriptableObject
{
    public string questName;
    [TextArea] public string questStory;

    [Header("Objectives")]
    public List<QuestObjective> objectives;

    [Header("Rewards")]
    public int coinsReward;
    public ItemDefinitionSO itemReward;
    public int itemRewardAmount = 1;
}