using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;

                enemy.TakeDamage(10, knockbackDirection);
            }
        }

        
        
    }
}
