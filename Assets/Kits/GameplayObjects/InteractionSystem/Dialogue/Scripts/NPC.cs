using System.Collections;
using UnityEngine;

public class NPC : BaseInteractable

{
    [SerializeField, TextArea(1, 5)] private string[] frases;
    [SerializeField] private float timeBetweenLetters = 0.05f;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
    private bool talking = false;
    private int indexActual = -1;


    protected override void OnInteract(GameObject requester)
    {
        dialogueBox.SetActive(true);
        if (!talking)
        {
            indexActual++;
            if (indexActual >= frases.Length)
            {
                dialogueBox.SetActive(false);
                indexActual = -1;
                talking = false;
                dialogueText.text = "";    
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
        dialogueText.text = frases[indexActual];
        talking = false;
    }

    IEnumerator WritePhrase()
    {
        talking = true;
        dialogueText.text = "";
        char[] caracters = frases[indexActual].ToCharArray();
        foreach (char c in caracters)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(timeBetweenLetters);

        }
        talking = false;
    }
}
