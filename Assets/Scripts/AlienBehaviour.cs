using UnityEngine;

public class AlienBehaviour : MonoBehaviour
{
    public int health = 100; // Alien's health
    public int score = 10;
    // Variables for circular motion
    public float radius = 5f; // Radius of the circle
    public float speed = 1f;  // Speed of the circular motion
    private float angle = 0f; // Current angle in radians
    private Vector3 spawnPosition; // Spawn position to use as the center of the circle
    public LogicScript logic; // Reference to the LogicScript

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPosition = transform.position; // Store the initial position as the spawn position
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>(); // Find the LogicScript
        Debug.Log(logic.playerScore); // Log the player's score
    }

    // Update is called once per frame
    void Update()
    {
        // Circular motion logic
        angle += speed * Time.deltaTime; // Increment the angle based on speed
        float x = Mathf.Cos(angle) * radius; // Calculate x offset
        float y = Mathf.Sin(angle) * radius; // Calculate z offset
        transform.position = new Vector3(spawnPosition.x + x, spawnPosition.y + y, spawnPosition.z); // Update position
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            // Get the damage value from the bullet's script
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                health -= bullet.damage;

                // Destroy the bullet after collision
                Destroy(collision.gameObject);

                // Check if health is zero or below
                if (health <= 0)
                {
                    Destroy(gameObject); // Destroy the alien
                    logic.addScore(score); // Add score to the player
                }
            }
        }
    }
}
