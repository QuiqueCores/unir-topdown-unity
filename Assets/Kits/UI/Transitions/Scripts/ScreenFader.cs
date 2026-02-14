using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponentInChildren<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public IEnumerator FadeOut(float duration)
    {
        canvasGroup.blocksRaycasts = true;
        yield return FadeTo(1f, duration);
    }

    public IEnumerator FadeIn(float duration)
    {
        yield return FadeTo(0f, duration);
        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator FadeTo(float target, float duration)
    {
        float start = canvasGroup.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = 1f;
            if (duration > 0f)
            {
                k = Mathf.Clamp01(t / duration);
            }
            canvasGroup.alpha = Mathf.Lerp(start, target, k);
            yield return null;
        }

        canvasGroup.alpha = target;
    }
}