using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI Components")]
    public Image fillImage;
    public CanvasGroup canvasGroup;

    private float fadeDuration = 0.2f;
    private Coroutine hideRoutine;
    private Canvas worldCanvas;
    public void Setup()
    {
        worldCanvas=GetComponent<Canvas>();
        worldCanvas.worldCamera = Camera.main;
        HideInstant();
    }

    public void UpdateHealthBar(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }

    public void HideInstant()
    {
        if (hideRoutine != null) StopCoroutine(hideRoutine);
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }

    public void ShowTemporary(float duration = 1.5f)
    {
        if (hideRoutine != null) StopCoroutine(hideRoutine);

        canvasGroup.gameObject.SetActive(true);
        canvasGroup.DOFade(1f, fadeDuration);
        hideRoutine = StartCoroutine(HideAfterDelay(duration));
    }

    private System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            canvasGroup.gameObject.SetActive(false);
        });
    }
}
