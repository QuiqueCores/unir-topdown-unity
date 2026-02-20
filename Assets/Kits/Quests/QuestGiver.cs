using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField] private QuestDefinition quest;
    [SerializeField] DialogueSO giveQuestDialogue;
    [SerializeField] DialogueSO ongoingQuestDialogue;
    [SerializeField] DialogueSO completedQuestDialogue;
    protected override void OnInteract(GameObject requester)
    {
        // 1. Antes de que o pai (NPC) faga nada, cambiamos o diálogo segundo o estado
        UpdateDialogueBasedOnQuest();

        // 2. Agora chamamos á lóxica do pai. 
        // Como o pai usa a variable 'conversations', el lerá o que acabamos de inxectar.
        base.OnInteract(requester);

        // 3. Lóxica de aceptar/completar a misión
        HandleQuestLogic(requester);
    }

    private void UpdateDialogueBasedOnQuest()
    {
        QuestStatus status = QuestManager.instance.ActiveQuests.Find(q => q.QuestData == quest);

        // Accedemos á variable 'conversations' do pai (que é protected)
        // Sobrescribimos a primeira posición (ou a que uses) co diálogo axeitado
        if (status == null)
        {
            // Estado: Ofrecer misión
            this.conversations[0] = giveQuestDialogue;
        }
        else if (status.isCompleted)
        {
            // Estado: Misión lista para entregar
            this.conversations[0] = ongoingQuestDialogue;
        }
        else
        {
            // Estado: En curso pero non rematada
            this.conversations[0] = completedQuestDialogue;
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