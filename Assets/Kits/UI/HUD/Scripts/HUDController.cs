using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField] private FloatEventChannelSO healthNormalisedChannel;
    [SerializeField] private StringEventChannelSO promptChannel;

    [Header("UI Refs")]
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text promptText;

    private void OnEnable()
    {
        healthNormalisedChannel.OnRaised += OnHealthChanged;
        promptChannel.OnRaised += OnPromptChanged;
    }

    private void OnDisable()
    {
        healthNormalisedChannel.OnRaised -= OnHealthChanged;
        promptChannel.OnRaised -= OnPromptChanged;
    }

    private void OnHealthChanged(float normalised)
    {
        healthFill.fillAmount = Mathf.Clamp01(normalised);
    }

    private void OnPromptChanged(string msg)
    {
        promptText.text = msg ?? string.Empty;
        promptText.gameObject.SetActive(!string.IsNullOrWhiteSpace(promptText.text));
    }
}