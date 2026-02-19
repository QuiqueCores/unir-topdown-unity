using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    
    public string responseText;
    public DialogueSO nextDialogue;
    public bool makesHostile;
}
