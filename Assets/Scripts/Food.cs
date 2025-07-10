using UnityEngine;

public class Food : MonoBehaviour, ICollectible
{
    public SpriteRenderer spriteRenderer;
    public float healAmount = 5f;

    private void Start()
    {
        spriteRenderer.sprite = FoodManager.Instance.GetRandomFood();
    }

    public void OnCollect(GameObject collector)
    {
        IHealth health = collector.GetComponent<IHealth>();
        if (health != null)
        {
            health.Heal(healAmount);
        }
        FoodManager.Instance.NotifyFoodEaten();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollect(other.gameObject);
        }
    }
}
