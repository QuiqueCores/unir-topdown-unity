using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifeTime = 60f;

    private Vector3 direction;

    public void Initialize(Vector3 moveDirection, float moveSpeed, float scale, float alpha)
    {
        direction = moveDirection.normalized;
        speed = moveSpeed;

        Vector3 baseScale = transform.localScale;
        transform.localScale = baseScale * scale;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
