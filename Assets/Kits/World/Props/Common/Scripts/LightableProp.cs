using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightableProp : MonoBehaviour, ILightable
{
    [Header("Main Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite lightedSprite;
    [SerializeField] private Sprite notLightedSprite;
    [SerializeField] private Light2D lightObject;

    [Header("Optional Shadow")]
    [SerializeField] private SpriteRenderer shadowSpriteRenderer;
    [Range(0f, 1f)]
    [SerializeField] private float shadowOpacityWhenOff = 0.35f;

    [Header("Initial State")]
    [SerializeField] private bool initiallyTurnedOn = false;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (lightObject == null) lightObject = GetComponentInChildren<Light2D>();
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

        if (lightObject != null)
            lightObject.enabled = true;

        SetShadowOpacity(1f);
    }

    public void TurnOff()
    {
        if (spriteRenderer != null && notLightedSprite != null)
            spriteRenderer.sprite = notLightedSprite;

        if (lightObject != null)
            lightObject.enabled = false;

        SetShadowOpacity(shadowOpacityWhenOff);
    }

    private void SetShadowOpacity(float alpha)
    {
        if (shadowSpriteRenderer == null) return;

        Color c = shadowSpriteRenderer.color;
        c.a = Mathf.Clamp01(alpha);
        shadowSpriteRenderer.color = c;
    }
}
