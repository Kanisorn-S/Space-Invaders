using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private float maxZ = 100f;
    public int damage = 50;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
        if (transform.position.z > maxZ || transform.position.z < -maxZ)
        {
            Destroy(gameObject);
        }
    }
}
