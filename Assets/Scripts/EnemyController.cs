using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public CDTimer cdTimer;
    public float newTimeRemaining = 10f;
    public int health = 2;
    public float moveSpeed = 2f;

    public float knockbackForce = 5f; 
    public float knockbackDuration = 0.2f; 
    private bool isKnockedBack = false; 
    protected SpriteRenderer spriteRenderer;  

    private Rigidbody2D rb; 
    public float damageCooldown = 1f;  
    public bool canTakeDamage = true;
    protected bool IsInvincible { get; set; }   
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  
    }

    public virtual void Move()
    {
        
        if (!isKnockedBack)
        {
            
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        
        if (canTakeDamage && !IsInvincible)
        {
            health -= damage;
            Debug.Log("Enemy took damage. Current health: " + health);

            
            StartCoroutine(FlashRed());

           
            if (health <= 0)
            {
                Die();
            }
            else
            {
                
                StartCoroutine(ApplyKnockback(knockbackDirection));
                StartCoroutine(DamageCooldown()); 
            }
        }
    }
    

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection)
    {
        isKnockedBack = true;  
        rb.velocity = new Vector2(knockbackDirection.x * knockbackForce, knockbackForce);  

        yield return new WaitForSeconds(knockbackDuration);  

        isKnockedBack = false; 
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;  

        yield return new WaitForSeconds(0.2f);  

        spriteRenderer.color = Color.white; 
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;  
        yield return new WaitForSeconds(damageCooldown);  
        canTakeDamage = true; 
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1); 
            }

            
        }
    }

    public abstract void Die();
  

    public abstract void UseAbility(); 
}

