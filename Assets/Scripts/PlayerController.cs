using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    //private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    public int maxHealth = 2;
    private int currentHealth;
    private bool hasAbilities = false;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip player when moving left-right, while preserving Y and Z scales
        Vector3 scale = transform.localScale;

        if (horizontalInput > 0.01f)
            scale.x = Mathf.Abs(scale.x);  // Face right
        else if (horizontalInput < -0.01f)
            scale.x = -Mathf.Abs(scale.x);  // Face left

        transform.localScale = scale;

        // Set animation parameters (adjust as needed for your Animator)
        //anim.SetBool("run", horizontalInput != 0);
        //anim.SetBool("grounded", isGrounded());

        // Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 5;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }
    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(moveX * speed, body.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            //anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
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



    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!hasAbilities && body.velocity.y < 0)
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
        speed = enemy.moveSpeed;
   
        enemy.UseAbility();


       
        hasAbilities = true;


        // Bounce the player up after stomp
        body.velocity = new Vector2(body.velocity.x, jumpPower * 0.5f);
    }


    void Die()
    {
        Debug.Log("Player Died!");
        //restart level
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }


}


