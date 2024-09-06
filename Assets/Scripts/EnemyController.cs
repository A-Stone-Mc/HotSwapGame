using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public int health = 2;
    public float moveSpeed = 2f;

    public virtual void Move()
    {
        
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();  

    public abstract void UseAbility(); 
}

