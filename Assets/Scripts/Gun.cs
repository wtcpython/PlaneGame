using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float spwanRate = 1;

    private float timer = 0;

    void Start()
    {
        
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1 / spwanRate)
        {
            timer -= 1 / spwanRate;
            SpawnBullet();
        }
    }

    void SpawnBullet()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
