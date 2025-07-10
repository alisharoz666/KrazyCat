using UnityEngine;
public interface IHealth
{
    void TakeDamage(float amount);
    void Heal(float amount);
}
public interface ICollectible
{
    void OnCollect(GameObject collector);
}
