using UnityEngine;

[CreateAssetMenu(menuName = "NPC/Dialogue")]
public class DialogueSO : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] lines;
    public DialogueChoice[] choices;

}
