using UnityEngine;

public class NPC : BaseInteractable
{
    [SerializeField] private DialogueSO[] conversations;

    [SerializeField] private BaseEnemy enemyBehaviour;

    private int conversationIndex = 0;
    private DialogueSO activeDialogue;

    private void Awake()
    {
        enemyBehaviour.enabled = false;
    }

    protected override void OnInteract(GameObject requester)
    {
        if (!DialogueUIManager.Instance.IsOpen)
        {
            GameManager.Instance.SetState(GameState.Dialogue);

            activeDialogue = conversations[conversationIndex];
            DialogueUIManager.Instance.OpenDialogue(
                activeDialogue,
                onChoiceSelected: OnChoiceSelected
            );
        }

        DialogueUIManager.Instance.Advance();
    }

    private void OnChoiceSelected(DialogueChoice choice)
    {
        if (choice.nextDialogue != null)
        {
            activeDialogue = choice.nextDialogue;

            conversations[conversationIndex] = activeDialogue;

            DialogueUIManager.Instance.SetDialogue(activeDialogue);
            DialogueUIManager.Instance.Advance();
        }

        if (choice.makesHostile)
        {
            BecomeHostile();
        }
    }

    private void BecomeHostile()
    {
        DialogueUIManager.Instance.Close();
        GameManager.Instance.SetState(GameState.Playing);

        enabled = false;
        enemyBehaviour.enabled = true;
    }

    public bool IsDialogueOpen()
    {
        return DialogueUIManager.Instance != null && DialogueUIManager.Instance.IsOpen;
    }

    public void SetConversationAt(int index, DialogueSO dialogue)
    {
        if (conversations == null)
        {
            Debug.LogWarning($"{name}: conversations array is null.", this);
            return;
        }

        if (index < 0 || index >= conversations.Length)
        {
            Debug.LogWarning($"{name}: conversation index {index} out of range (len {conversations.Length}).", this);
            return;
        }

        conversations[index] = dialogue;
    }
}