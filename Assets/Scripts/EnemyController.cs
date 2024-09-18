using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public CDTimer cdTimer;
    public float newTimeRemaining = 10f;
    public int health = 2;
    public float moveSpeed = 2f;


    public virtual void Move()
    {
        
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage. Current health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //deal damage
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1); // Damage the player
            }

            
        }
    }

    public abstract void Die();
  

    public abstract void UseAbility(); 
}

