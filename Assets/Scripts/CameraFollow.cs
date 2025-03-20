using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public Vector3 offset = new Vector3(0, 5, -7); // Offset from the player's position
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
