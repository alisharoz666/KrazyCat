using UnityEngine;

public class EnemyContact : MonoBehaviour
{
    public float damageToPlayer = 10f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IHealth player = other.GetComponent<IHealth>();
            if (player != null)
            {
                player.TakeDamage(damageToPlayer);
            }
        }
    }

}
