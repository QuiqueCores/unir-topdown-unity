using UnityEngine;

public class MockInteractable : BaseInteractable
{
    protected override void OnInteract(GameObject requester)
    {
        Debug.Log("<color=green> [Interaction Succeeded] </color>");
        Debug.Log("The statue smiles with satisfaction. You didn't just bring the right item; you brought hope. The gods of Debugging are pleased!");
    }
}