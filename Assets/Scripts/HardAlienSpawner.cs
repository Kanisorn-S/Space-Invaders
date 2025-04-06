using UnityEngine;

public class HardAlienSpawner : MonoBehaviour
{
    public GameObject alienPrefab; // Reference to the bullet prefab
    public float spawnRate = 5f; // Rate at which aliens are spawned
    private float timer = 0f;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = 0f;
    public float maxY = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        GameObject hardAlien = Instantiate(alienPrefab, new Vector3(x, y, transform.position.z), transform.rotation);
        HardAlien alienScript = hardAlien.GetComponent<HardAlien>();
        if (alienScript != null)
        {
            alienScript.speed = 1f;
        }
        else
        {
            Debug.LogError("HardAlien script not found on the prefab!");
        }
    }
}


