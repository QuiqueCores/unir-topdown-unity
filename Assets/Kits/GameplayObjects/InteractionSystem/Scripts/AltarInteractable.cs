using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class AltarInteractable : BaseInteractable
{

    [Header("Visuals")]

    [SerializeField] GameObject altarOff;
    [SerializeField] GameObject altarOn;
    [SerializeField] Light2D altarLight;

    bool activated;

    protected override void OnInteract(GameObject requester)
    {
        if (activated)
            return;

        ActivateAltar();
    }

    void ActivateAltar()
    {
        activated = true;

        if (altarOff != null)
            altarOff.SetActive(false);

        if (altarOn != null)
            altarOn.SetActive(true);

        if (altarLight != null)
            altarLight.enabled = true;

        Debug.Log("Altar activated");
    }

}
