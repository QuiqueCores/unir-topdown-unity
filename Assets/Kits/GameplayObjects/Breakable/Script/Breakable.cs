using UnityEngine;

public class Breakable : BaseInteractable
{
    protected override void OnInteract(GameObject requester)
    {
        Debug.Log("<color=green> [Interaction Succeeded] </color>");
        Debug.Log("The block is destroyed and the path is now open");
        Destroy(gameObject);
    }
}
