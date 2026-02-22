using UnityEngine;

public class Breakable : BaseInteractable
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;
    protected override void OnInteract(GameObject requester)
    {
        Debug.Log("<color=green> [Interaction Succeeded] </color>");
        Debug.Log("The block is destroyed and the path is now open");
        audioSource.PlayOneShot(sounds[0]);
        
        Destroy(gameObject, 0.3f);
    }
}
