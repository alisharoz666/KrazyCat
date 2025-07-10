using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance;

    [Header("Food Setup")]
    public List<Sprite> foodSprites;

    [Header("UI")]
    public TMP_Text foodCounterText;

    private int totalFood;
    private int foodEaten;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Count total food at the start
        totalFood = FindObjectsOfType<Food>().Length;
        UpdateUI();
    }

    public Sprite GetRandomFood()
    {
        return foodSprites[Random.Range(0, foodSprites.Count)];
    }

    public void NotifyFoodEaten()
    {
        foodEaten++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (foodCounterText != null)
        {
            foodCounterText.text = "Food: "+$"{foodEaten} / {totalFood}";
        }
    }
}
