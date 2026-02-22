using UnityEngine;

public class BaseSkeleton : BaseEnemy
{

    [SerializeField] AudioClip hurtSound;
    protected override void ExecuteAI()
    {
        base.ExecuteAI();
        Move(distance.normalized);
    }

    public override void TakeDamage(int amount)
    {
        Debug.Log("Skeleton recibió daño de: " + amount);

        if (amount <= 0) return;

        if (hurtSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hurtSound);
            Debug.Log("Reproduciendo sonido");
        }

        base.TakeDamage(amount);
    }

}
