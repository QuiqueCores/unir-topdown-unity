using System.Collections;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [SerializeField] GameObject cloudPrefab;


    [SerializeField] float spawnIntervalMin = 4f;
    [SerializeField] float spawnIntervalMax = 8f;

    [SerializeField] float spawnOffset = 2f;

    [SerializeField] float minSpeed = 0.5f;
    [SerializeField] float maxSpeed = 2f;

    public Vector2 scaleRange = new Vector2(200f, 400f);
    public Vector2 alphaRange = new Vector2(0.1f, 0.5f);


    [SerializeField] bool spawnAboveGround = true;
    [SerializeField] bool spawnBelowGround = true;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnCloud();
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
        }
    }

    void SpawnCloud()
    {
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector3 leftSpawn = new Vector3(
            cam.transform.position.x - camWidth / 2 - spawnOffset,
            Random.Range(cam.transform.position.y - camHeight / 2,
                         cam.transform.position.y + camHeight / 2),
            0);

        GameObject cloudObj = Instantiate(cloudPrefab, leftSpawn, Quaternion.identity);

        SpriteRenderer sr = cloudObj.GetComponent<SpriteRenderer>();

        // Elegir capa (arriba o abajo)
        if (spawnAboveGround && spawnBelowGround)
        {
            if (Random.value > 0.5f)
                sr.sortingLayerName = "PropsHigh";
            else
                sr.sortingLayerName = "Background";
        }
        else if (spawnAboveGround)
            sr.sortingLayerName = "PropsHigh";
        else
            sr.sortingLayerName = "Background";

        float speed = Random.Range(minSpeed, maxSpeed);
        float scale = Random.Range(scaleRange.x, scaleRange.y);
        float alpha = Random.Range(alphaRange.x, alphaRange.y);

        Vector3 direction = Vector3.right;

        cloudObj.GetComponent<Clouds>().Initialize(direction, speed, scale, alpha);
    }

}
