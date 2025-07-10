using UnityEngine;

public class Key : MonoBehaviour, ICollectible
{
    public void OnCollect(GameObject collector)
    {
        KeyManager.Instance.AddKey();
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
