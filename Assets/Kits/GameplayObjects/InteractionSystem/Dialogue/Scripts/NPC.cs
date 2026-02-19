using System.Collections;
using UnityEngine;

public class NPC : BaseInteractable

{
    //[SerializeField] private GameObject manager;
    //[SerializeField] private NPCManager npcManager ;
    [SerializeField] private NPCManagerSO npcManager;
    [SerializeField] private DialogueSO[] conversations;

    [SerializeField, TextArea(1, 5)] private string[] frases;


    [SerializeField] private float timeBetweenLetters = 0.05f;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
    [Header("Choices UI")]
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;

    [SerializeField] private BaseEnemy enemyBehaviour;

    private bool talking = false;
    private int conversationIndex = 0;
    private int indexActual = -1;


    private void Awake()
    {
        //npcManager = manager.GetComponent<NPCManager>();
        enemyBehaviour.enabled = false;
    }
    protected override void OnInteract(GameObject requester)
    {
        npcManager.ChangePlayerState(true);
        dialogueBox.SetActive(true);
        if (!talking)
        {
            indexActual++;
            var currentDialogue = conversations[conversationIndex];

            if (indexActual >= currentDialogue.lines.Length)
            {
                if (currentDialogue.choices != null && currentDialogue.choices.Length > 0)
                {
                    ShowChoices(currentDialogue.choices);
                    return;
                }

                //conversationIndex = Mathf.Min(conversationIndex + 1, conversations.Length - 1);
                dialogueBox.SetActive(false);
                indexActual = -1;
                talking = false;
                dialogueText.text = "";
                npcManager.ChangePlayerState(false);
            }
            else
            {
                StartCoroutine(WritePhrase());
            }

        }
        else
        {
            CompletePhrase();
        }

    }

    private void CompletePhrase()
    {
        StopAllCoroutines();
        dialogueText.text = conversations[conversationIndex].lines[indexActual];
        talking = false;
    }

    IEnumerator WritePhrase()
    {
        talking = true;
        dialogueText.text = "";
        char[] caracters = conversations[conversationIndex].lines[indexActual].ToCharArray();
        foreach (char c in caracters)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(timeBetweenLetters);

        }
        talking = false;
    }

    void ShowChoices(DialogueChoice[] choices)
    {
        choicesPanel.SetActive(true);

        foreach (Transform child in choicesPanel.transform)
            Destroy(child.gameObject);

        foreach (var choice in choices)
        {
            var btn = Instantiate(choiceButtonPrefab, choicesPanel.transform);

            btn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = choice.responseText;

            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                ChooseOption(choice);
            });
        }
    }

    void ChooseOption(DialogueChoice choice)
    {
        choicesPanel.SetActive(false);

        if (choice.nextDialogue != null)
        {
            conversations[conversationIndex] = choice.nextDialogue;
            indexActual = -1;
            OnInteract(gameObject);
        }

        if (choice.makesHostile)
        {
            BecomeHostile();
        }
    }

    void BecomeHostile()
    {
        dialogueBox.SetActive(false);
        npcManager.ChangePlayerState(false);
        this.GetComponent<NPC>().enabled = false;
        enemyBehaviour.enabled = true;

        
        
    }
}
