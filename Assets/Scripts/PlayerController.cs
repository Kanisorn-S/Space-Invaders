using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 100; // Player's health
    public float moveSpeed = 10f;
    public float tiltSpeed = 70f;
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform bulletSpawnPoint; // Position where bullets are spawned
    public int maxTiltAngle = 50;
    public int offsetTilt = 5;
    public LogicScript logic; // Reference to the LogicScript
    private Quaternion homeRotation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        homeRotation = transform.rotation; // Store the initial rotation
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>(); // Find the LogicScript
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            movement += Vector3.up * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            movement += Vector3.down * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            movement += Vector3.left * moveSpeed * Time.deltaTime;
            float zTilt = transform.eulerAngles[2];
            Debug.Log(zTilt);
            if (zTilt < (maxTiltAngle + 90)) {
                transform.Rotate(Vector3.forward, tiltSpeed * Time.deltaTime);
            }
        } else if (Input.GetKey(KeyCode.D))
        {
            float zTilt = transform.eulerAngles[2];
            movement += Vector3.right * moveSpeed * Time.deltaTime;
            if (zTilt > (90 - maxTiltAngle)) {
                transform.Rotate(Vector3.forward, -tiltSpeed * Time.deltaTime);
            }
        } else {
            // Smoothly reset tilt to normal orientation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, homeRotation, tiltSpeed * Time.deltaTime);
        }

        // if (Input.GetMouseButton(1)) // Right-click
        //     movement += Vector3.back * moveSpeed * Time.deltaTime;

        // if (Input.GetMouseButton(0)) // Left-click
        //     movement += Vector3.forward * moveSpeed * Time.deltaTime;

        transform.Translate(movement, Space.World);

        // Shooting bullets
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Debug.Log(bullet.layer);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 6)
        {
            // Get the damage value from the bullet's script
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                health -= bullet.damage;
                logic.updatePlayerHealth(false, bullet.damage);

                // Destroy the bullet after collision
                Destroy(collision.gameObject);

                // Check if health is zero or below
                if (health <= 0)
                {
                    // Destroy the alien
                    Destroy(gameObject); 
                }
            }
        }
    }
}
