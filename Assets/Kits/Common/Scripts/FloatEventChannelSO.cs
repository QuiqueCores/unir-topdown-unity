using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Kits/Events/Float Event Channel")]
public class FloatEventChannelSO : ScriptableObject
{
    public event Action<float> OnRaised;

    public void Raise(float value)
    {
        OnRaised?.Invoke(value);
    }
}