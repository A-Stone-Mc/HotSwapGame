using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyController : EnemyController
{
    public Transform pointA; // Patrol point A
    public Transform pointB; // Patrol point B
    public Transform firePoint; // The point where the fire is shot from
    public float detectionRange = 8f; // Detection range for the player
    public float fireDuration = 5f; // Duration for which fire is shot
    public float fireCooldown = 3f; // Cooldown between fire attacks
    public float fireDamageInterval = 2f;
    public float fireDamageRadius = 2f; // Interval at which the fire damages the player

    private bool playerInRange = false;
    private bool isFiring = false;
    private float fireCooldownTimer = 0f;
    private Animator animator;
    private bool isFacingRight = true;
    public GameObject rightFireStreamPrefab; // The right-facing fire particle system prefab
    public GameObject leftFireStreamPrefab;

    public Transform player; // Reference to the player
    private Vector3 targetPosition;

    private PlayerController playerController;
    private GameObject activeFireStream;

    private void Start()
    {
        animator = GetComponent<Animator>();
        targetPosition = pointB.position;  // Initialize to the second patrol point
    }

    private void Update()
    {
        Patrol();
        CheckPlayerInRange();

        // Handle fire cooldown
        if (fireCooldownTimer > 0)
        {
            fireCooldownTimer -= Time.deltaTime;
        }

        if (playerInRange && fireCooldownTimer <= 0 && !isFiring)
        {
            // Trigger fire animation if player is in range and cooldown has passed
            animator.SetTrigger("IsFiring");
        }

        UpdateAnimations(); // Update the animations based on movement and firing
    }

    // Patrol between two points
    private void Patrol()
    {
        if (!playerInRange && !isFiring)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Check if the enemy reached the patrol point
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
                Flip();
            }
        }
    }

    // Check if the player is in range
    private void CheckPlayerInRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        playerInRange = distanceToPlayer <= detectionRange;

        if (playerInRange)
        {
            FacePlayer(player);
        }
    }

    // Face the player by flipping sprite
    private void FacePlayer(Transform player)
    {
        Vector3 direction = player.position - transform.position;
        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // Coroutine to handle shooting fire and its cooldown
    private IEnumerator ShootFire()
    {
        isFiring = true;

        // Instantiate the appropriate fire stream based on the direction the enemy is facing
        if (isFacingRight)
        {
            activeFireStream = Instantiate(rightFireStreamPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            activeFireStream = Instantiate(leftFireStreamPrefab, firePoint.position, firePoint.rotation);
        }

        StartCoroutine(DamagePlayerOverTime());

        yield return new WaitForSeconds(fireDuration);

        // Stop the fire particle system and destroy it
        Destroy(activeFireStream);

        fireCooldownTimer = fireCooldown;  // Start cooldown
        isFiring = false;
    }

    // Apply damage to player every few seconds while firing
    private IEnumerator DamagePlayerOverTime()
    {
        while (isFiring)
        {
            // Check if the player is within the fire's damage radius and in the correct direction
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(firePoint.position, fireDamageRadius);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    Vector3 directionToPlayer = player.position - firePoint.position;
                    bool playerInFront = (directionToPlayer.x > 0 && isFacingRight) || (directionToPlayer.x < 0 && !isFacingRight);

                    if (playerInFront)
                    {
                        PlayerController playerController = hitCollider.GetComponent<PlayerController>();
                        if (playerController != null)
                        {
                            playerController.TakeFireDamage(1);  // Deal 1 damage
                        }
                    }
                }
            }

            yield return new WaitForSeconds(fireDamageInterval);  // Wait before dealing damage again
        }
    }

    // Animation event to call this method at the appropriate time in the animation
    public void OnFireAnimationEvent()
    {
        StartCoroutine(ShootFire());
    }

    // Update animations for patrol, idle, and firing states
    private void UpdateAnimations()
    {
        // If the enemy is firing, no need to check other animations
        if (isFiring) return;

        // Play walking animation if moving, otherwise play idle
        bool isWalking = Vector3.Distance(transform.position, targetPosition) > 0.1f;
        animator.SetBool("IsWalking", isWalking);
    }

    // Flip the enemy and fire stream direction
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        // Flip the enemy sprite
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }


    public override void UseAbility() {}

    public override void Die()
    {
        if (playerController != null)
        {
            playerController.GainAbilitiesFromEnemy(this);
        }
        cdTimer.timeRemaining = newTimeRemaining;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, fireDamageRadius);
    }
}
