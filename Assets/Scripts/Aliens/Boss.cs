using UnityEngine;

public class Boss : AlienBase
{
    public override int score { get; set; } = 200; 
    public override int health { get; set; } = 1000; 
    private float healthBarScale;
    public GameObject healthBar;
    private LogicScript logicScript; // Reference to the LogicScript

    public override void Start()
    {
        base.Start(); // Call the base class Start method
        healthBarScale = healthBar.transform.localScale.x; // Store the initial scale of the health bar
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>(); // Find the LogicScript
    }
    public override void Update()
    {
        base.Update(); // Call the base class Update method
        // Update the health bar scale based on the current health
        if (health > 0)
        {
            float newScale = healthBarScale * ((float)health / 1000f); // Calculate the new scale based on current health
            healthBar.transform.localScale = new Vector3(newScale, healthBar.transform.localScale.y, healthBar.transform.localScale.z); // Update the health bar scale
        } else {
            logicScript.BossDefeated(); // Call the BossDefeated method in LogicScript
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (health <= 0)
        {
            logicScript.BossDefeated(); // Call the BossDefeated method in LogicScript
        }
        base.OnCollisionEnter(collision); // Call the base class OnCollisionEnter method
    }
}
