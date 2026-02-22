using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightDisco : MonoBehaviour
{
    [SerializeField] private List<Light2D> lights = new List<Light2D>();

    [Header("Intensity Settings")]
    [SerializeField] private float minIntensity = 0.8f;
    [SerializeField] private float maxIntensity = 2f;
    [SerializeField] private float intensitySpeed = 3f;

    [Header("Color Settings")]
    [SerializeField] private float colorSpeed = 0.5f;

    private class LightData
    {
        public Light2D light;
        public float intensityOffset;
        public float hueOffset;
        public float speedMultiplier;
    }

    private List<LightData> lightDataList = new List<LightData>();

    void Awake()
    {
        if (lights.Count == 0)
            lights.AddRange(GetComponentsInChildren<Light2D>(true));

        foreach (var l in lights)
        {
            if (!l) continue;

            LightData data = new LightData
            {
                light = l,
                intensityOffset = Random.Range(0f, 100f),
                hueOffset = Random.Range(0f, 1f),
                speedMultiplier = Random.Range(0.8f, 1.2f)
            };

            lightDataList.Add(data);
        }
    }

    void Update()
    {
        foreach (var data in lightDataList)
        {
            if (!data.light) continue;

            float wave = Mathf.Sin((Time.time + data.intensityOffset) * intensitySpeed * data.speedMultiplier);
            wave = wave * 0.5f + 0.5f;
            data.light.intensity = Mathf.Lerp(minIntensity, maxIntensity, wave);

            float hue = Mathf.Repeat(Time.time * colorSpeed * data.speedMultiplier + data.hueOffset, 1f);
            data.light.color = Color.HSVToRGB(hue, 1f, 1f);
        }
    }
}