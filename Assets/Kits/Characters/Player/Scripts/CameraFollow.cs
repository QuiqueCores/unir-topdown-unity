using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] PlayerCharacter player;
    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed = 8f;
    [SerializeField] Vector3 offset;
    [SerializeField] float lookOffsetDistance = 2f;
    [SerializeField] float lookSmoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector2 lookDir = player != null ? player.lookDirection : Vector2.zero;

        Vector3 desiredPosition = (Vector3)(lookDir * lookOffsetDistance);

        offset = Vector3.Lerp(
            offset,
            desiredPosition,
            lookSmoothSpeed * Time.deltaTime
        );

        desiredPosition = target.position + offset;
        desiredPosition.z = transform.position.z;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    private void Start()
    {
        if (PlayerPersistent.Instance != null)
        {
            player = PlayerPersistent.Instance.Character;
            target = player.transform;
        }
    }
}
