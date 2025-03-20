using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float tiltSpeed = 50f;

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
            transform.Rotate(Vector3.forward, tiltSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector3.right * moveSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, -tiltSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButton(1)) // Right-click
            movement += Vector3.back * moveSpeed * Time.deltaTime;

        if (Input.GetMouseButton(0)) // Left-click
            movement += Vector3.forward * moveSpeed * Time.deltaTime;

        transform.Translate(movement, Space.World);
    }
}
