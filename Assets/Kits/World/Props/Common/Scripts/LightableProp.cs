using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightableProp : MonoBehaviour, ILightable
{
    [Header("Main Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite lightedSprite;
    [SerializeField] private Sprite notLightedSprite;
    [SerializeField] private List<Light2D> lights = new List<Light2D>();

    [Header("Optional Shadow")]
    [SerializeField] private SpriteRenderer shadowSpriteRenderer;
    [Range(0f, 1f)]
    [SerializeField] private float shadowOpacityWhenOff = 0.35f;

    [Header("Initial State")]
    [SerializeField] private bool initiallyTurnedOn = false;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        lights.Clear();
        GetComponentsInChildren(true, lights);
    }

    private void Awake()
    {
        if (initiallyTurnedOn) TurnOn();
        else TurnOff();
    }

    public void TurnOn()
    {
        if (spriteRenderer != null && lightedSprite != null)
            spriteRenderer.sprite = lightedSprite;

        SetLightsEnabled(true);

        SetShadowOpacity(1f);
    }

    public void TurnOff()
    {
        if (spriteRenderer != null && notLightedSprite != null)
            spriteRenderer.sprite = notLightedSprite;

        SetLightsEnabled(false);

        SetShadowOpacity(shadowOpacityWhenOff);
    }

    private void SetLightsEnabled(bool enabled)
    {
        if (lights == null) return;

        for (int i = 0; i < lights.Count; i++)
        {
            var l = lights[i];
            if (l == null) continue;
            l.enabled = enabled;
        }
    }

    private void SetShadowOpacity(float alpha)
    {
        if (shadowSpriteRenderer == null) return;

        Color c = shadowSpriteRenderer.color;
        c.a = Mathf.Clamp01(alpha);
        shadowSpriteRenderer.color = c;
    }
}
