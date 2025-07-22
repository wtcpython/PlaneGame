using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 3.0f;

    void Start()
    {
        
    }
    
    void Update()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));

        if (transform.position.y > Constants.DestroyYLimit)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(1);
            Destroy(this.gameObject);
        }
    }
}
