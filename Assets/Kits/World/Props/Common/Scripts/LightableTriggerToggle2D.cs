using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class LightableTriggerToggle2D : MonoBehaviour
{
    [Header("Targets")]
    [Tooltip("GameObjects that have components implementing ILightable.")]
    [SerializeField] private List<GameObject> lightableGameObjects = new List<GameObject>();

    [Header("Player Detection")]
    [Tooltip("Player tag to match on trigger enter/exit.")]
    [SerializeField] private string playerTag = "Player";

    [Header("Behavior")]
    [Tooltip("If true, all targets will be turned off when the player leaves the trigger.")]
    [SerializeField] private bool turnOffOnLeave = false;
    [Tooltip("If true the collider will be disabled after activation.")]
    [SerializeField] private bool disableColliderAfterUse = true;

    private readonly Dictionary<ILightable, bool> knownIsOn = new Dictionary<ILightable, bool>();
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        CacheTargets();
    }

    private void OnValidate()
    {
        CacheTargets();
    }

    private void CacheTargets()
    {
        knownIsOn.Clear();

        foreach (var go in lightableGameObjects)
        {
            if (go == null) continue;

            var components = go.GetComponents<MonoBehaviour>();
            foreach (var mb in components)
            {
                if (mb is ILightable lightable)
                {
                    if (!knownIsOn.ContainsKey(lightable))
                        knownIsOn.Add(lightable, false);
                }
            }
        }
    }

    private bool IsPlayer(Collider2D other)
        => other != null && other.CompareTag(playerTag);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsPlayer(other)) return;

        foreach (var kvp in new List<ILightable>(knownIsOn.Keys))
        {
            var lightable = kvp;
            if (lightable == null) continue;

            bool isOn = knownIsOn[lightable];
            if (isOn)
            {
                lightable.TurnOff();
                knownIsOn[lightable] = false;
            }
            else
            {
                lightable.TurnOn();
                knownIsOn[lightable] = true;
            }
        }

        if (_collider != null)
            _collider.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!turnOffOnLeave) return;
        if (!IsPlayer(other)) return;

        foreach (var lightable in new List<ILightable>(knownIsOn.Keys))
        {
            if (lightable == null) continue;

            lightable.TurnOff();
            knownIsOn[lightable] = false;
        }
    }
}
