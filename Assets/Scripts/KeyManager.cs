using UnityEngine;
using TMPro;  // Required for TextMeshPro

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;

    [SerializeField] private TMP_Text keyCounterText;

    private int keyCount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddKey()
    {
        keyCount++;
        UpdateUI();
        Debug.Log("Key collected. Total: " + keyCount);
    }

    public bool UseKey()
    {
        if (keyCount > 0)
        {
            keyCount--;
            UpdateUI();
            Debug.Log("Key used. Remaining: " + keyCount);
            return true;
        }
        Debug.Log("No keys to use.");
        return false;
    }

    public int GetKeyCount() => keyCount;

    private void UpdateUI()
    {
        if (keyCounterText != null)
            keyCounterText.text = "Keys: " + keyCount;
    }
}
