using System.Collections;
using UnityEngine;

public class Breakable : BaseInteractable
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;
    protected override void OnInteract(GameObject requester)
    {
        StartCoroutine(BreakSequence());
    }

    private IEnumerator BreakSequence()
    {
        Debug.Log("<color=green> [Interaction Succeeded] </color>");
        if (sounds.Length > 0) audioSource.PlayOneShot(sounds[0]);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
