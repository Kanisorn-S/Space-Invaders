using UnityEngine;

public class AlienBase : MonoBehaviour
{
    // Alien's Information
    public virtual int health { get; set; } = 50; 
    public virtual int score { get; set; } = 10;
    public Vector3 spawnPosition; // Spawn position to use as the center of the circle
    public float speed = 1f; 
    public float baseSpeed = 1f;
    public float fireRate = 5f;
    private float timer = 0f;
    // Reference to Main Logic
    public virtual LogicScript logic { get; set; } // Reference to the LogicScript; 
    public bool isBoss = false;
    public float normalSpeed; // Normal speed of the alien
    public float slowedSpeed; // Slowed speed of the alien
    public BaseAlienSpawner spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        // Store the initial position as the spawn position
        spawnPosition = transform.position; 
        // Find the LogicScript
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>(); 
        normalSpeed = baseSpeed + logic.wave * 0.1f; // Increase speed based on wave
        slowedSpeed = normalSpeed / 2f; // Set the slowed speed to half of the normal speed
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (logic.powerUpIndex == 4)
        {
            speed = slowedSpeed; // Set speed to slowed speed if power-up is active
        } else {
            speed = normalSpeed; // Set speed to normal speed if power-up is not active
        }
      // Call the move function
      move();
      // Call the shoot function
        // if (timer > fireRate)
        // {
        //     shoot();
        //     timer = 0f;
        // } else {
        //     timer += Time.deltaTime;
        // }
    }

    public virtual void move()
    {
        // Move the alien
        // transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    // public virtual void shoot()
    // {
    //     // Instantiate a bullet
    //     GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    //     // Get the bullet's script
    //     Bullet bulletScript = bullet.GetComponent<Bullet>();
    //     // Set the bullet's damage value
    //     bulletScript.damage = 10;
    // }

    public virtual void OnCollisionEnter(Collision collision)
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
                    if (isBoss)
                    {
                        logic.bossDefeated();
                    } else {
                        spawner.alienDeathCount++; // Increment the alien death count in the spawner
                    }
                    logic.alienDeath();
                    // Destroy the alien
                    Destroy(gameObject); 
                    // Add score to the player
                    logic.addScore(score); 
                }
            }
        }
    }
}
