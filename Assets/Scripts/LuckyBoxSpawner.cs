using UnityEngine;

public class LuckyBoxSpawner : MonoBehaviour
{
    public GameObject luckyBoxPrefab; // Reference to the lucky box prefab
    public float spawnRate = 15f; // Rate at which lucky boxes are spawned
    private float timer = 0f;
    public float minX = -7f;
    public float maxX = 7f;
    public float minY = 0f;
    public float maxY = 7f;
    private int spawnCount = 0; // Number of lucky boxes spawned
    public int maxSpawnCount = 3; // Maximum number of lucky boxes to spawn
    public int baseSpawnCount = 3; // Base number of lucky boxes to spawn
    public LogicScript logic; // Reference to the LogicScript
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>(); // Find the LogicScript
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > spawnRate)
        {
            SpawnLuckyBox();
            timer = 0f;
        } else {
            timer += Time.deltaTime;
        }
        
    }

    void SpawnLuckyBox()
    {
        // Instantiate a new lucky box at the spawner's position
        if (spawnCount >= maxSpawnCount)
        {
            return; // Stop spawning if the maximum count is reached
        }
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Instantiate(luckyBoxPrefab, new Vector3(x, y, transform.position.z), transform.rotation);
        spawnCount++;
    }

    public void goToWave(int wave)
    {
        // Logic to go to the next wave
        spawnCount = 0; // Reset spawn count for the new wave
        maxSpawnCount = baseSpawnCount + (1 * wave); // Increase max spawn count based on wave
    }
}
