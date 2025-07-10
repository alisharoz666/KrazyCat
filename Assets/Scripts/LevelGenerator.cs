using UnityEngine;

public class LevelDesigner : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite[] roadSprites;
    public Sprite[] buildingSprites;
    public Sprite[] windowSprites;
    public Sprite lockIconSprite;
    public Sprite[] foodSprites;
    public Sprite fishSprite;

    [Header("Prefabs")]
    public GameObject treePrefab; // ✅ NEW: full prefab (branches + leaves)
    public GameObject keyPrefab;
    public GameObject[] enemyPrefabs;
    public GameObject thunderPrefab;
    public GameObject playerSpawnPoint;

    [Header("Settings")]
    public int numberOfBuildings = 5;
    public int windowsPerBuilding = 3;
    public int numberOfKeys = 15;
    public int numberOfEnemies = 6;
    public int numberOfThunders = 3;
    public int numberOfTrees = 10;
    public Vector2 levelBounds = new Vector2(100, 10);

    void Start()
    {
        GenerateVisualLevel();
    }

    void GenerateVisualLevel()
    {
        float spacing = levelBounds.x / numberOfBuildings;
        float currentX = 0;

        // 1. Roads
        for (int i = 0; i < roadSprites.Length; i++)
        {
            Vector2 pos = new Vector2(i * 20, -1.5f);
            CreateSpriteObject(roadSprites, pos, "Road", 0);
        }

        // 2. Trees using prefab
        for (int i = 0; i < numberOfTrees; i++)
        {
            float x = Random.Range(0f, levelBounds.x);
            float y = -0.5f;
            if (treePrefab != null)
                Instantiate(treePrefab, new Vector2(x, y), Quaternion.identity);
        }

        // 3. Buildings with windows
        for (int i = 0; i < numberOfBuildings; i++)
        {
            Vector2 buildingPos = new Vector2(currentX + 2f, 0f);
            GameObject building = CreateSpriteObject(buildingSprites, buildingPos, "Building", 1);

            for (int w = 0; w < windowsPerBuilding; w++)
            {
                Vector2 windowOffset = new Vector2(Random.Range(0.5f, 2.5f), Random.Range(2f, 4.5f));
                Vector2 windowPos = buildingPos + windowOffset;

                GameObject window = CreateSpriteObject(windowSprites, windowPos, "Window", 2);

                // Lock icon
                if (lockIconSprite != null)
                {
                    GameObject lockIcon = new GameObject("LockIcon");
                    SpriteRenderer sr = lockIcon.AddComponent<SpriteRenderer>();
                    sr.sprite = lockIconSprite;
                    sr.sortingOrder = 3;
                    lockIcon.transform.SetParent(window.transform);
                    lockIcon.transform.localPosition = new Vector2(0, 0.4f);
                }

                // Food (hidden for now)
                if (Random.value < 0.3f)
                {
                    Vector2 foodPos = windowPos + Vector2.up * 0.5f;
                    GameObject foodGO = new GameObject("Food");
                    SpriteRenderer fr = foodGO.AddComponent<SpriteRenderer>();
                    fr.sprite = (Random.value < 0.2f && fishSprite != null)
                        ? fishSprite
                        : foodSprites[Random.Range(0, foodSprites.Length)];
                    fr.sortingOrder = 2;
                    fr.enabled = false; // hidden for now
                    foodGO.transform.position = foodPos;
                }
            }

            currentX += spacing;
        }

        // 4. Keys behind trees or near enemies
        for (int i = 0; i < numberOfKeys; i++)
        {
            float x = Random.Range(1f, levelBounds.x - 2f);
            float y = 0f;
            Instantiate(keyPrefab, new Vector2(x, y), Quaternion.identity);
        }

        // 5. Enemies
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float x = Random.Range(2f, levelBounds.x - 2f);
            float y = 1f;
            int index = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[index], new Vector2(x, y), Quaternion.identity);
        }

        //// 6. Thunders
        //for (int i = 0; i < numberOfThunders; i++)
        //{
        //    float x = Random.Range(0, levelBounds.x);
        //    float y = levelBounds.y + 3f;
        //    Instantiate(thunderPrefab, new Vector2(x, y), Quaternion.identity);
        //}

        // 7. Player
        Instantiate(playerSpawnPoint, new Vector2(1, 0.5f), Quaternion.identity);
    }

    GameObject CreateSpriteObject(Sprite[] sprites, Vector2 position, string name, int sortingOrder)
    {
        if (sprites.Length == 0) return null;
        GameObject go = new GameObject(name);
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprites[Random.Range(0, sprites.Length)];
        sr.sortingOrder = sortingOrder;
        go.transform.position = position;
        return go;
    }
}
