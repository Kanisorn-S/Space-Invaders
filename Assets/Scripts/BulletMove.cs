using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private float maxZ = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        if (transform.position.z > maxZ || transform.position.z < -maxZ)
        {
            Destroy(gameObject);
        }
    }
}
