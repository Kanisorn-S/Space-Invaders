using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float tiltSpeed = 70f;
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform bulletSpawnPoint; // Position where bullets are spawned
    public int maxTiltAngle = 20;
    public int offsetTilt = 5;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            if (zTilt < maxTiltAngle || zTilt > (360 - maxTiltAngle - offsetTilt)) {
                transform.Rotate(Vector3.forward, tiltSpeed * Time.deltaTime);
            }
        } else if (Input.GetKey(KeyCode.D))
        {
            float zTilt = transform.eulerAngles[2];
            movement += Vector3.right * moveSpeed * Time.deltaTime;
            if (zTilt > (360 - maxTiltAngle) || zTilt < maxTiltAngle + offsetTilt) {
                transform.Rotate(Vector3.forward, -tiltSpeed * Time.deltaTime);
            }
        } else {
            // Smoothly reset tilt to normal orientation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), tiltSpeed * Time.deltaTime);
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
}
