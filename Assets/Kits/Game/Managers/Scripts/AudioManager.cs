using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        AudioListener.volume = masterVolume;
    }
}