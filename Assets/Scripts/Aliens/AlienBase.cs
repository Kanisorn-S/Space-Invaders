using UnityEngine;

public class AlienBase : MonoBehaviour
{
    // Alien's Information
    public int health = 100; 
    public int score = 10;
    public float speed = 1f; 
    public Vector3 spawnPosition; 
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 5f;
    private float timer = 0f;
    // Reference to Main Logic
    public LogicScript logic; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store the initial position as the spawn position
        spawnPosition = transform.position; 
        // Find the LogicScript
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>(); 
    }

    // Update is called once per frame
    void Update()
    {
      // Call the move function
      move();
      // Call the shoot function
        if (timer > fireRate)
        {
            shoot();
            timer = 0f;
        } else {
            timer += Time.deltaTime;
        }
    }

    public virtual void move()
    {
        // Move the alien
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    public virtual void shoot()
    {
        // Instantiate a bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        // Get the bullet's script
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        // Set the bullet's damage value
        bulletScript.damage = 10;
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
                    // Destroy the alien
                    Destroy(gameObject); 
                    // Add score to the player
                    logic.addScore(score); 
                }
            }
        }
    }
}
