using UnityEngine;

public class QuestGiver : NPC
{
    [Header("Quest")]
    [SerializeField] private QuestDefinition quest;
    [SerializeField] DialogueSO giveQuestDialogue;
    [SerializeField] DialogueSO ongoingQuestDialogue;
    [SerializeField] DialogueSO completedQuestDialogue;
    protected override void OnInteract(GameObject requester)
    {
        if (!IsDialogueOpen())
        {
            UpdateDialogueBasedOnQuest();

            base.OnInteract(requester);

            HandleQuestLogic(requester);
        }

        else
        {
            base.OnInteract(requester);
        }
    }

    private void UpdateDialogueBasedOnQuest()
    {
        QuestStatus status = QuestManager.instance.ActiveQuests.Find(q => q.QuestData == quest);

        if (status == null)
        {
            this.SetConversationAt(0, giveQuestDialogue);
        }
        else
        {
            QuestManager.instance.SyncQuestWithInventory(status);

            if (status.isCompleted)
            {
                this.SetConversationAt(0, completedQuestDialogue);
            }
            else
            {
                this.SetConversationAt(0, ongoingQuestDialogue);
            }
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
        if (inventory != null)
        {   
            if (quest.itemReward1 != null)
            {
                inventory.Add(quest.itemReward1, quest.itemReward1Amount);
            }
            if (quest.itemReward2 != null)
            {
                inventory.Add(quest.itemReward2, quest.itemReward2Amount);
            }
            QuestManager.instance.ActiveQuests.RemoveAll(q => q.QuestData == quest);
        }
    }
}