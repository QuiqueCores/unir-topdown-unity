using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField] private QuestDefinition quest;
    [SerializeField] DialogueSO giveQuestDialogue;
    [SerializeField] DialogueSO ongoingQuestDialogue;
    [SerializeField] DialogueSO completedQuestDialogue;
    protected override void OnInteract(GameObject requester)
    {
        UpdateDialogueBasedOnQuest();

        base.OnInteract(requester);

        HandleQuestLogic(requester);
    }

    private void UpdateDialogueBasedOnQuest()
    {
        QuestStatus status = QuestManager.instance.ActiveQuests.Find(q => q.QuestData == quest);

        if (status == null)
        {
            this.SetConversationAt(0, giveQuestDialogue);
        }
        else if (status.isCompleted)
        {
            this.SetConversationAt(0, ongoingQuestDialogue);
        }
        else
        {
            this.SetConversationAt(0, completedQuestDialogue);
        }
    }

    private void HandleQuestLogic(GameObject player)
    {
        QuestStatus status = QuestManager.instance.ActiveQuests.Find(q => q.QuestData == quest);

        if (status == null)
        {
            QuestManager.instance.AcceptQuest(quest);
        }
        else if (status.isCompleted)
        {
            GiveReward(player);
        }
    }

    private void GiveReward(GameObject player)
    {
        var inventory = player.GetComponentInChildren<InventorySystem>();
        if (inventory != null && quest.itemReward != null)
        {
            inventory.Add(quest.itemReward, quest.itemRewardAmount);
            QuestManager.instance.ActiveQuests.RemoveAll(q => q.QuestData == quest);
        }
    }
}