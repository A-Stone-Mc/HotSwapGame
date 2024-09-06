using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxHealth = 2;
    private int currentHealth;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool hasAbilities = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        Move();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("Player hit! Current Health: " + currentHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!hasAbilities && rb.velocity.y < 0)
            {
                EnemyController enemy = collision.GetComponent<EnemyController>();
                enemy.TakeDamage(1);
                if (enemy.health <= 0)
                {
                    GainAbilitiesFromEnemy(enemy);
                }
            }
        }
    }

    void GainAbilitiesFromEnemy(EnemyController enemy)
    {
        moveSpeed = enemy.moveSpeed;
    
        enemy.UseAbility();

        
        hasAbilities = true;

        // Bounce the player up after stomp
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.5f);
    }

    void Die()
    {
        Debug.Log("Player Died!");
        //restart level
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}


