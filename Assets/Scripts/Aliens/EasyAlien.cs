using UnityEngine;

public class EasyAlien : AlienBase
{
    private float maxXAlien = 10f; // Maximum x position
    private float minXAlien = -10f; // Minimum x position
    private int update = 1;

    public override void Start()
    {
        base.Start(); // Call the base class Start method
        spawner = GameObject.FindGameObjectWithTag("EasySpawner").GetComponent<BaseAlienSpawner>(); // Find the AlienSpawner
    }
    public override void move()
    {
        // Move side to side
        if (transform.position.x >= maxXAlien)
        {
            update = -1;
        }
        else if (transform.position.x <= minXAlien)
        {
            update = 1;
        }
        transform.position = new Vector3(transform.position.x + (speed * update * Time.deltaTime), transform.position.y, transform.position.z);
    }
}
