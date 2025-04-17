using UnityEngine;

public class HardAlien : AlienBase
{
    public float radius = 5f; // Radius of the circle
    private float angle = 0f; // Current angle in radians
    public override int score { get; set; } = 50 + (10 * GameData.startingWave); // Hard alien score
    public override int health { get; set; } = 200; // Hard alien health
    public override void Start()
    {
        base.Start(); // Call the base class Start method
        spawner = GameObject.FindGameObjectWithTag("HardSpawner").GetComponent<BaseAlienSpawner>(); // Find the AlienSpawner
    }
    public override void move()
    {
        // Circular motion logic
        angle += speed * Time.deltaTime; // Increment the angle based on speed
        float x = Mathf.Cos(angle) * radius; // Calculate x offset
        float y = Mathf.Sin(angle) * radius; // Calculate z offset
        transform.position = new Vector3(spawnPosition.x + x, spawnPosition.y + y, spawnPosition.z); // Update position
    }
}
