using UnityEngine;

public class AttackFX : MonoBehaviour
{
    public float damage = 20f;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            IHealth enemyHealth = col.GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
