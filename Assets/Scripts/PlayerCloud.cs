using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloud : MonoBehaviour
{
    public int damage = 1; // Damage dealt to the player
    public float damageInterval = 2f; // Time between damage ticks

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                // Start damaging the player
                StartCoroutine(DamageEnemy(enemy));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop damaging the player when they leave the cloud
            StopAllCoroutines();
        }
    }

    private IEnumerator DamageEnemy(EnemyController enemy)
    {
        while (true)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            enemy.TakeDamage(damage, knockbackDirection);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}