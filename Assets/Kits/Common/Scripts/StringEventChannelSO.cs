using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Kits/Events/String Event Channel")]
public class StringEventChannelSO : ScriptableObject
{
    public event Action<string> OnRaised;

    public void Raise(string value)
    {
        OnRaised?.Invoke(value);
    }
}