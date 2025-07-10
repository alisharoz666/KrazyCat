using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour, IHealth
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    [Header("UI")]
    public HealthBarUI healthBar;

    [Header("Damage Feedback")]
    public SpriteRenderer spriteRenderer; // Assign manually or auto-fetch
    public Color damageColor = Color.red;
    public float flashDuration = 0.1f;
    public bool useScalePunch = true;

    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.Setup();
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            healthBar.ShowTemporary();
        }

        ShowDamageFeedback();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            healthBar.ShowTemporary();
        }
    }

    private void ShowDamageFeedback()
    {
        if (spriteRenderer == null) return;

        // Flash color
        spriteRenderer.DOColor(damageColor, 0.05f).OnComplete(() =>
        {
            spriteRenderer.DOColor(originalColor, flashDuration);
        });

        // Optional scale punch
        if (useScalePunch)
        {
            transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 10, 1);
        }
    }

    private void Die()
    {
        isDead = true;

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        transform.DOMoveY(transform.position.y + 1f, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.DOMoveY(transform.position.y - 5f, 1f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                Destroy(gameObject);
                if(gameObject.CompareTag("Player"))
                {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            });
        });
    }
}
