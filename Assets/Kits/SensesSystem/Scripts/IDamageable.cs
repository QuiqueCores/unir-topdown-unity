using UnityEngine;

public interface IDamageable : IVisible2D
{
    void TakeDamage(int amount);

    Transform Transform { get; }
}
