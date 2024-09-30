using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifetime = 2f;
    private Vector2 moveDirection;

    private void Start()
    {

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                enemy.TakeDamage(damage, knockbackDirection);  // Deal damage to the enemy
            }
            Destroy(gameObject);  // Destroy the laser on impact
        }
    }
}
