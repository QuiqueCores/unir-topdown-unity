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
    //public int coinsReward;
    public ItemDefinitionSO itemReward1;
    public int itemReward1Amount = 1;
    public ItemDefinitionSO itemReward2;
    public int itemReward2Amount = 1;
}