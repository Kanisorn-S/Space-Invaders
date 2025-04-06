using UnityEngine;

public class MediumAlien : AlienBase
{
    public override int health { get; set; } = 100; // Medium alien health
    public override int score { get; set; } = 30; // Medium alien score
    public override void move()
    {
        // Straight motion logic
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);

        if (transform.position.z <= -5f)
        {
            logic.gameOver();
            Destroy(gameObject); // Destroy the alien
        }

    }
}
