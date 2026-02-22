using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBlink : MonoBehaviour
{
    [SerializeField] private List<Light2D> lights = new List<Light2D>();
    [SerializeField] private float interval = 0.5f;

    private float timer;

    void Awake()
    {
        // Si no asignaste manualmente, las busca automáticamente
        if (lights.Count == 0)
            lights.AddRange(GetComponentsInChildren<Light2D>(true));
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            foreach (var l in lights)
                if (l) l.enabled = !l.enabled;

            timer = 0f;
        }
    }
}
