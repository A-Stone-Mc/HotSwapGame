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
    [SerializeField] private float rotationDuration = 0.5f; 
    public GameObject lightningBoltPrefab; // Prefab for player lightingn
     public Transform dropPoint; 
    public float damageCooldown = 1f;  // Cooldown time between damage
    private float damageCooldownTimer = 0f;

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

    // Laser ability
    public GameObject laserPrefab; // Prefab for player laser
    public Transform laserShootPoint; // Point where laser is shot from
    public float laserCooldown = 2f; // Cooldown time between laser shots
    private float laserCooldownTimer;


    [Tooltip("Scale of Flying Enemy Type")]
    public Vector3 newFlyingScale; //scale of flying enemy sprite
    [Tooltip("Scale of Laser Enemy Type")]
    public Vector3 newLaserScale;
    public LayerMask platformLayer;  // Reference to the platform layer

    public Sprite flyingTypeSprite; 
    public Sprite laserTypeSprite;
    private SpriteRenderer spriteRenderer;
    private bool canShootLaser = false; 
    public float knockbackForce = 10f;  // Force of knockback
    public float knockbackDuration = 0.2f; // Duration of knockback
    private bool isKnockedBack = false; // Is the player currently being knocked back?
    private Vector2 lastMovementDirection;
    private List<Collider2D> disabledEnemyColliders = new List<Collider2D>();
     private bool isInvulnerable = false;  

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
        if (!isKnockedBack) 
        {
            horizontalInput = Input.GetAxis("Horizontal");

            // Flip player when moving left-right
            Vector3 scale = transform.localScale;
            if (horizontalInput > 0.01f)
                scale.x = Mathf.Abs(scale.x);  // Face right
            else if (horizontalInput < -0.01f)
                scale.x = -Mathf.Abs(scale.x);  // Face left

            transform.localScale = scale;

            
            if (horizontalInput != 0)
            {
                lastMovementDirection = new Vector2(horizontalInput, 0);
            }

            if (damageCooldownTimer > 0)
            {
                damageCooldownTimer -= Time.deltaTime;
            }

            // Normal movement
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
       
    
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

            if (canShootLaser) //LASER SHOOTING
            {
                laserCooldownTimer -= Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.E) && laserCooldownTimer <= 0)
                {
                    ShootLaser();
                    laserCooldownTimer = laserCooldown;
                }
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
             StartCoroutine(RotatePlayer());
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

    private IEnumerator RotatePlayer()
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, 360);  // Rotate 360 degrees

        // Rotate the player over the duration of the jump
        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            yield return null;
        }

        // Ensure the rotation is exactly 360 degrees at the end
        transform.rotation = targetRotation;
    }

    private void ResetAbilities()
    {
        // Reset abilities
        canFly = false;
        canShootLaser = false;
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
        ResetAbilities();
        canFly = true;
    }

    private IEnumerator TempInvulnerability(float duration)
    {
        isInvulnerable = true;  // Enable invulnerability
        yield return new WaitForSeconds(duration);  // Wait for the given duration
        isInvulnerable = false;  // Disable invulnerability
    }

    private void ShootLaser()
    {
        // Instantiate the laser at the shoot point
        GameObject laserBeam = Instantiate(laserPrefab, laserShootPoint.position, laserShootPoint.rotation);

        // Determine the direction based on the player's facing direction
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Pass the direction to the laser projectile
        PlayerLaser beamScript = laserBeam.GetComponent<PlayerLaser>();
        if (beamScript != null)
        {
            beamScript.SetDirection(direction);  // Set the direction for the laser
        }

        // Flip the laser prefab if the player is facing left
        if (direction == Vector2.left)
        {
            laserBeam.transform.localScale = new Vector3(-laserBeam.transform.localScale.x, laserBeam.transform.localScale.y, laserBeam.transform.localScale.z);
        }
    }
    public void GainLaserAbilities()
    {
        ResetAbilities();
        spriteRenderer.sprite = laserTypeSprite;  // Change to laser sprite
        transform.localScale = newLaserScale;  
        Vector2 newSize = new Vector2(14.80273f, 14.70244f);  
        boxCollider.size = newSize;
        boxCollider.offset = new Vector2(0.2726059f, -1.744905f);  // Adjust collider offset

        canShootLaser = true;  // Enable shooting laser ability
        Debug.Log("Player gained laser shooting abilities.");
        jumpPower = 22f;
    }

    public void TakeDamage(int damage)
    {
       
        if (!isInvulnerable && damageCooldownTimer <= 0)
        {
            currentHealth -= damage;
            damageCooldownTimer = damageCooldown;

            // Apply knockback
            StartCoroutine(ApplyKnockback());

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(FlashRed());  
                Debug.Log("Player hit! Current Health: " + currentHealth);
            }
        }
    }

    private IEnumerator ApplyKnockback()
    {
        isKnockedBack = true;  // Disable player movement

        // Temporarily disable collisions with nearby enemies
        DisableEnemyColliders(true);

        Vector2 knockbackDirection = -lastMovementDirection.normalized;  // Opposite of last movement direction
        body.velocity = new Vector2(knockbackDirection.x * knockbackForce, knockbackForce);  // Apply knockback force

        yield return new WaitForSeconds(knockbackDuration);  // Wait for knockback duration

        isKnockedBack = false;  // Re-enable player movement

        // Re-enable collisions with nearby enemies
        DisableEnemyColliders(false);
    }

    private void DisableEnemyColliders(bool disable)
    {
        // Find all nearby enemies
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 5f);

        if (disable)
        {
            // Disable colliders and store them in the list
            foreach (Collider2D enemyCollider in nearbyEnemies)
            {
                if (enemyCollider.CompareTag("Enemy"))
                {
                    enemyCollider.enabled = false;  // Disable the enemy's collider
                    disabledEnemyColliders.Add(enemyCollider);  // Add to the list for later re-enabling
                }
            }
        }
        else
        {
            // Re-enable colliders that were disabled
            foreach (Collider2D enemyCollider in disabledEnemyColliders)
            {
                if (enemyCollider != null)  // Ensure the collider still exists
                {
                    enemyCollider.enabled = true;  // Re-enable the collider
                }
            }
            disabledEnemyColliders.Clear();  // Clear the list after re-enabling
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
                if (enemy != null)
                {
                    Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                    enemy.TakeDamage(1, knockbackDirection); 

                    
                    body.velocity = new Vector2(body.velocity.x, jumpPower * 0.7f);

                    
                    StartCoroutine(TempInvulnerability(0.3f)); 
                }
            }
        }
    }


    public void GainAbilitiesFromEnemy(EnemyController enemy)
    {
        Debug.Log("Player gained abilities from: " + enemy.GetType().Name);
        speed = enemy.moveSpeed;
   
        enemy.UseAbility();

        if (enemy is FlyingEnemyController)
        {
            spriteRenderer.sprite = flyingTypeSprite;  // Change to flying enemy sprite
            transform.localScale = newFlyingScale;
            Vector2 newSize = new Vector2(13.299f, 13.78256f); // New width and height of flying collider
            boxCollider.size = newSize;
        
            boxCollider.offset = new Vector2(-0.5520077f, -1.562553f);
            GainFlyingAbilities();
        }

        else if (enemy is LaserEnemyController)
        {
            GainLaserAbilities(); 
        }
       
        hasAbilities = true;
        Debug.Log("Player gained abilities from enemy!");


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
        
        if (FuelManager.Instance != null)
        {
            FuelManager.Instance.ResetFuel();
        }
  
        Debug.Log("Player Died!");
        //restart level
        FindObjectOfType<LevelTimer>().ResetTimer();
        UnityEngine.SceneManagement.SceneManager.LoadScene(
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        if (FuelManager.Instance != null)
        {
            FuelManager.Instance.ResetFuel();
        }
    }


}


