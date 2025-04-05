using UnityEngine;

public class HardAlien : AlienBase
{
    public float radius = 5f; // Radius of the circle
    private float angle = 0f; // Current angle in radians
    public override int score { get; set; } = 50; // Hard alien score
    public override void move()
    {
        // Circular motion logic
        angle += speed * Time.deltaTime; // Increment the angle based on speed
        float x = Mathf.Cos(angle) * radius; // Calculate x offset
        float y = Mathf.Sin(angle) * radius; // Calculate z offset
        transform.position = new Vector3(spawnPosition.x + x, spawnPosition.y + y, spawnPosition.z); // Update position
    }
}
