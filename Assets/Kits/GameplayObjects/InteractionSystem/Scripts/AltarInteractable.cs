using UnityEngine;
using UnityEngine.Rendering.Universal;


public class AltarInteractable : BaseInteractable
{

    [Header("Visuals")]

    [SerializeField] GameObject altarOff;
    [SerializeField] GameObject altarOn;
    [SerializeField] Light2D altarLight;

    [Header("Reward")]

    [SerializeField] GameObject rewardPrefab;

    [SerializeField] Transform rewardSpawnPoint;

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

        SpawnReward();

        Debug.Log("Altar activated");
    }

    void SpawnReward()
    {
        if (rewardPrefab == null)
            return;

        Vector3 pos = rewardSpawnPoint != null
            ? rewardSpawnPoint.position
            : transform.position;

        Instantiate(rewardPrefab, pos, Quaternion.identity);

    }

}
