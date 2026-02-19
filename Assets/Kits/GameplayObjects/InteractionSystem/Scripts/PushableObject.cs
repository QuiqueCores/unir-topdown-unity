using UnityEngine;

public class PushableObject : BaseInteractable
{
    [SerializeField] float pushForce = 5f;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void OnInteract(GameObject requester)
    {
        Vector2 direction = (transform.position - requester.transform.position).normalized;
        rb.AddForce(direction * pushForce, ForceMode2D.Impulse);
    }
}
