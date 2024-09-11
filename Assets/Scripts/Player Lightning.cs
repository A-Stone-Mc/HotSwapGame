using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightning : MonoBehaviour
{
    public int damage = 1;
    public float speed = 5f;
    
    void Start()
    {
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject); 
        }

        
        
    }
}
