using UnityEngine;

public class AlienBehaviour : MonoBehaviour
{
    public int health = 100; // Alien's health

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            // Get the damage value from the bullet's script
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            Debug.Log("Bullet damage: " + bullet.damage);
            if (bullet != null)
            {
                health -= bullet.damage;

                // Destroy the bullet after collision
                Destroy(collision.gameObject);

                // Check if health is zero or below
                if (health <= 0)
                {
                    Destroy(gameObject); // Destroy the alien
                }
            }
        }
    }
}
