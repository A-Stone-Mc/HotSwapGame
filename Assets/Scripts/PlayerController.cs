using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    public GameObject lightningBoltPrefab; // Prefab for player lightingn
     public Transform dropPoint; 

    private Rigidbody2D body;
    //private Animator anim;
    private BoxCollider2D boxCollider;
    public BoxCollider2D feetCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    public int maxHealth = 2;
    private int currentHealth;
    private bool hasAbilities = false;
    private bool canFly = false; 
    private float flySpeed = 2f; //speed of flight powers
    public float dropInterval = 1f; // Time between player drops
    private float dropTimer;

    public LayerMask platformLayer;  // Reference to the platform layer

    public Sprite flyingTypeSprite; 
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        // If the player presses "S" or "Down Arrow" drop through platform
    
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(DropThroughPlatform());
        }


        if (canFly && Input.GetKey(KeyCode.Space))
        {
            Fly();  // Fly when the holding space
        }
        else
        {
            body.gravityScale = 5;  // Return to reg gravity
        }

        if (canFly == true)
        {
            dropTimer -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.E))
            {

                if (dropTimer <= 0)
                {

                    DropLightningBolt();
                    dropTimer = dropInterval;
                }
            }
        }
        // Normal movement
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Set animation
        //anim.SetBool("IsWalking", horizontalInput != 0);
       // anim.SetBool("IsInAir", !isGrounded());

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
            //anim.SetTrigger("Jump");
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

    private void Fly()
    {
        body.gravityScale = 0;  // zero gravity
        body.velocity = new Vector2(body.velocity.x, flySpeed);  //slowly upwards
    }

    private void DropLightningBolt()
    {
        Instantiate(lightningBoltPrefab, dropPoint.position, Quaternion.identity);
        
    }

    // Called when player kills flying enemy
    public void GainFlyingAbilities()
    {
        canFly = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        body.velocity = new Vector2(body.velocity.x, jumpPower * 0.3f);
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed()); //change color of spritee
            Debug.Log("Player hit! Current Health: " + currentHealth);
        }
    }



    private bool isGrounded()
    {
        
        LayerMask combinedLayerMask = groundLayer | platformLayer;

        
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, combinedLayerMask);
        
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

    private void OnTriggerEnter2D(Collider2D collision) //deal damage to enemy
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
                    if (enemy is FlyingEnemyController)  
                    {
                        GainFlyingAbilities();  
                    }
                }
            }
        }
    }


    void GainAbilitiesFromEnemy(EnemyController enemy)
    {
        speed = enemy.moveSpeed;
   
        enemy.UseAbility();

        if (enemy is FlyingEnemyController)
        {
            spriteRenderer.sprite = flyingTypeSprite;  // Change to flying enemy sprite
        }
       
        hasAbilities = true;


        // Bounce the player up after stomp
        body.velocity = new Vector2(body.velocity.x, jumpPower * 0.5f);
    }

    private IEnumerator FlashRed()
    {
        // Change the player's color to red
        spriteRenderer.color = Color.red;

        // Wait for 0.2 seconds 
        yield return new WaitForSeconds(0.2f);

        // Change the player's color back 
        spriteRenderer.color = Color.white;
    }

    private IEnumerator DropThroughPlatform()
    {
        
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), true);
        
        yield return new WaitForSeconds(0.5f);  

        // enable collision with platforms
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), false);
    }


    void Die()
    {
        Debug.Log("Player Died!");
        //restart level
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }


}


