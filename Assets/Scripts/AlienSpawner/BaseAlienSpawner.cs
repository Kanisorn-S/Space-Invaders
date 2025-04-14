using UnityEngine;

public class BaseAlienSpawner : MonoBehaviour
{
    public GameObject alienPrefab; // Reference to the bullet prefab
    public float spawnRate = 5f; // Rate at which aliens are spawned
    private float timer = 0f;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = 0f;
    public float maxY = 10f;
    private int spawnCount = 0; // Number of aliens spawned
    public int maxSpawnCount = 2; // Maximum number of aliens to spawn
    public int baseSpawnCount = 2; // Base number of aliens to spawn
    public LogicScript logic; // Reference to the LogicScript
    public int alienDeathCount = 0; // Number of aliens killed
    public bool cleared = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>(); // Find the LogicScript
        SpawnAlien();
        cleared = false;
        maxSpawnCount = baseSpawnCount + (1 * logic.wave); // Increase max spawn count based on wave
        Debug.Log("Max Spawn:" + maxSpawnCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > spawnRate)
        {
            SpawnAlien();
            timer = 0f;
        } else {
            timer += Time.deltaTime;
        }
        if ((alienDeathCount == maxSpawnCount) && !cleared)
        {
            cleared = true;
            Debug.Log("Cleared the wave!");
        }
    }

    void SpawnAlien()
    {
        // Instantiate a new alien at the spawner's position
        if (spawnCount >= maxSpawnCount)
        {
            return; // Stop spawning if the maximum count is reached
        }
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Instantiate(alienPrefab, new Vector3(x, y, transform.position.z), transform.rotation);
        spawnCount++;
        Debug.Log("Spawned: " + spawnCount);
    }

    public void goToWave(int wave)
    {
        // Logic to go to the next wave
        cleared = false; // Reset cleared status for the new wave
        alienDeathCount = 0;
        spawnCount = 0; // Reset spawn count for the new wave
        maxSpawnCount = baseSpawnCount + (1 * wave); // Increase max spawn count based on wave
        spawnRate = 5f - (wave * 0.8f); // Decrease spawn rate based on wave
        if (spawnRate < 1f) // Ensure spawn rate doesn't go below 1 second
        {
            spawnRate = 1f;
        }
        Debug.Log("New Max Spawn: " + maxSpawnCount);
    }
}


