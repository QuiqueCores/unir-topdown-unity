using UnityEngine;

public class TestQuestGiver : MonoBehaviour, IInteractable
{
    [Header("Quest Data")]
    [SerializeField] private QuestDefinition questToGive;

    public void Interact(GameObject requester)
    {
        if (questToGive == null)
        {
            return;
        }

        if (QuestManager.instance != null)
        {
            QuestManager.instance.AcceptQuest(questToGive);
            Debug.Log($"<color=cyan>[NPC]</color> It's dangerous to go alone! Take this... well, take this quest first! {questToGive.questName}");
        }
    }
}