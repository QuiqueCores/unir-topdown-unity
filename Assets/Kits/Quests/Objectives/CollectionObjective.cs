using UnityEngine;

[CreateAssetMenu(fileName = "CollectionObjective", menuName = "Quests/Objectives/Collection")]
public class CollectionObjective : QuestObjective
{
    public ItemDefinitionSO itemToCollect;
}
