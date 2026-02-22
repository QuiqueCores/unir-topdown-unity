using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIManager : PersistentSingleton<DialogueUIManager>
{
    [Header("UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Typewriter")]
    [SerializeField] private float timeBetweenLetters = 0.05f;
    [SerializeField] private AudioClip typeSound;
    [SerializeField] private AudioClip choiceSound;

    private Coroutine typingRoutine;
    private bool isTyping;

    private DialogueSO currentDialogue;
    private int lineIndex = -1;
    private Action onDialogueClosed;
    private Action<DialogueChoice> onChoicePicked;

    public bool IsOpen => dialogueBox != null && dialogueBox.activeSelf;

    public void OpenDialogue(
        DialogueSO dialogue,
        Action<DialogueChoice> onChoiceSelected = null)
    {
        if (dialogue == null)
        {
            Debug.LogWarning("DialogueUIManager.OpenDialogue called with null dialogue.");
            return;
        }

        currentDialogue = dialogue;
        lineIndex = -1;
        onChoicePicked = onChoiceSelected;

        choicesPanel.SetActive(false);
        dialogueBox.SetActive(true);
        dialogueText.text = "";
    }

    public void Advance()
    {
        if (!IsOpen || currentDialogue == null) return;

        if (isTyping)
        {
            CompleteLine();
            return;
        }

        lineIndex++;

        if (lineIndex >= currentDialogue.lines.Length)
        {
            if (currentDialogue.choices != null && currentDialogue.choices.Length > 0)
            {
                ShowChoices(currentDialogue.choices);
                return;
            }

            Close();
            return;
        }

        StartLine(currentDialogue.lines[lineIndex]);
    }

    private void StartLine(string line)
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            dialogueBox.GetComponent<AudioSource>()?.PlayOneShot(typeSound, 0.2f);
            yield return new WaitForSeconds(timeBetweenLetters);
        }

        isTyping = false;
        typingRoutine = null;
    }

    private void CompleteLine()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = null;

        if (currentDialogue != null && lineIndex >= 0 && lineIndex < currentDialogue.lines.Length)
            dialogueText.text = currentDialogue.lines[lineIndex];

        isTyping = false;
    }

    private void ShowChoices(DialogueChoice[] choices)
    {
        choicesPanel.SetActive(true);

        foreach (Transform child in choicesPanel.transform)
            Destroy(child.gameObject);

        foreach (var choice in choices)
        {
            var btn = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            btn.GetComponentInChildren<TMP_Text>().text = choice.responseText;

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                choicesPanel.SetActive(false);
                onChoicePicked?.Invoke(choice);
            });
        }
    }

    public void SetDialogue(DialogueSO newDialogue)
    {
        dialogueBox.GetComponent<AudioSource>()?.PlayOneShot(choiceSound, 0.2f);
        currentDialogue = newDialogue;
        lineIndex = -1;
        dialogueText.text = "";
        choicesPanel.SetActive(false);
    }

    public void Close()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = null;
        isTyping = false;

        dialogueBox.SetActive(false);
        choicesPanel.SetActive(false);
        dialogueText.text = "";

        currentDialogue = null;

        onChoicePicked = null;

        GameManager.Instance.SetState(GameState.Playing);
    }
}