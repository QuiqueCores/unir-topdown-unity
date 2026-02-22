using System.Collections;
using UnityEngine;

public class PushableObject : BaseInteractable
{
    [SerializeField] float moveDuration = 0.2f;
    [SerializeField] LayerMask blockingLayers;
    [SerializeField] AudioClip moveSound;
    AudioSource audioSource;

    bool isMoving = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnInteract(GameObject requester)
    {
        Debug.Log("PushableObject.OnInteract en " + name);

        PlayerCharacter player = requester.GetComponent<PlayerCharacter>();
        if (player == null)
        {
            Debug.Log("No hay PlayerCharacter");
            return;
        }

        Vector2 dir = player.lookDirection;

        Debug.Log("Dirección recibida: " + dir);

        if (dir == Vector2.zero)
        {
            Debug.Log("Dirección cero → CANCELADO");
            return;
        }

        TryPush(dir);
    }

    public void TryPush(Vector2 direction)
    {
        Debug.Log("---- TryPush en " + name);

        if (isMoving)
        {
            Debug.Log("Ya se está moviendo → CANCELADO");
            return;
        }

        float gridSize = 1f;
        Vector2 targetPosition = (Vector2)transform.position + direction * gridSize;

        Debug.Log("Intentando mover a: " + targetPosition);

        Collider2D hit = Physics2D.OverlapBox(
            targetPosition,
            Vector2.one * 0.9f,
            0f,
            blockingLayers
        );

        if (hit != null)
        {
            Debug.Log("Bloqueado por: " + hit.name);
            return;
        }

        if (audioSource != null && moveSound != null)
            audioSource.PlayOneShot(moveSound);

        Debug.Log("Movimiento permitido → INICIANDO LERP");

        StartCoroutine(SmoothMove(targetPosition));
    }

    IEnumerator SmoothMove(Vector2 targetPosition)
    {
        isMoving = true;

        Vector2 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector2(
            Mathf.Round(targetPosition.x),
            Mathf.Round(targetPosition.y)
        );

        Debug.Log("Movimiento completado. Nueva posición: " + transform.position);

        isMoving = false;
    }
}