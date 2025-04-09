using UnityEngine;

public class Boss : AlienBase
{
    public override int score { get; set; } = 200; 
    public override int health { get; set; } = 1000; 
    private int baseHealth = 1000; // Base health for the boss
    private float healthBarScale;
    public GameObject healthBar;

    public override void Start()
    {
        base.Start(); // Call the base class Start method
        healthBarScale = healthBar.transform.localScale.x; // Store the initial scale of the health bar
        isBoss = true; // Set isBoss to true
        health = baseHealth + (200 * logic.wave / 5); // Set the health to the base health
    }
    public override void Update()
    {
        base.Update(); // Call the base class Update method
        // Update the health bar scale based on the current health
        if (health > 0)
        {
            float newScale = healthBarScale * ((float)health / 1000f); // Calculate the new scale based on current health
            healthBar.transform.localScale = new Vector3(newScale, healthBar.transform.localScale.y, healthBar.transform.localScale.z); // Update the health bar scale
        }
    }
}
