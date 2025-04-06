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
    public int maxSpawnCount = 10; // Maximum number of aliens to spawn
    public LogicScript logic; // Reference to the LogicScript
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>(); // Find the LogicScript
        SpawnAlien();
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
    }
}


