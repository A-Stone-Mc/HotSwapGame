using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasEnemyController : EnemyController
{
    public Transform pointA; // Patrol point A
    public Transform pointB; // Patrol point B
    public Transform throwPoint; // The point from which the gas bomb is thrown
    public GameObject gasBombPrefab; // The gas bomb prefab
    public float throwForce = 10f; // The force at which the gas bomb is thrown
    public float detectionRange = 10f; // How close the player must be to throw a bomb
    public float attackCooldown = 5f; // Time between bomb throws
    public float PatrolmoveSpeed = 2f; // Speed of movement when patrolling
    private float attackCooldownTimer;

    private Vector3 targetPosition;
    private bool playerInRange = false;
    private PlayerController playerController;

    private Animator animator; // Reference to the Animator

    private void Start()
    {
        targetPosition = pointB.position; // Start patrolling toward point B
        attackCooldownTimer = attackCooldown;
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void Update()
    {
        Patrol();
        CheckPlayerInRange();
        if (playerInRange && attackCooldownTimer <= 0)
        {
            // Trigger the throw animation
            animator.SetTrigger("Throw");
            attackCooldownTimer = attackCooldown; // Reset the cooldown
        }
        attackCooldownTimer -= Time.deltaTime;
        UpdatePlayerReference();
    }

    private void UpdatePlayerReference()
    {
        if (playerController == null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        }
    }

    // Patrol logic between two points
    private void Patrol()
    {
        if (!playerInRange)
        {
            animator.SetBool("IsWalking", true); // Play walking animation

            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Switch direction when reaching the patrol point
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
                Flip(); // Flip direction when changing target position
            }
        }
        else
        {
            animator.SetBool("IsWalking", false); // Stop walking when the player is in range
        }
    }

    // Check if the player is within detection range
    private void CheckPlayerInRange()
    {
        if (playerController != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerController.transform.position);
            if (distanceToPlayer <= detectionRange)
            {
                playerInRange = true;
                FacePlayer(playerController.gameObject);
            }
            else
            {
                playerInRange = false;
            }
        }
    }

    // Face the direction of the player
    private void FacePlayer(GameObject player)
    {
        Vector3 direction = player.transform.position - transform.position;
        if (direction.x > 0)
        {
            // Face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            // Face left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // This method is called by the animation event at the right frame
    public void ThrowGasBomb()
    {
        if (playerController == null) return;

        // Create the gas bomb at the throw point
        GameObject gasBomb = Instantiate(gasBombPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = gasBomb.GetComponent<Rigidbody2D>();

        // Calculate the throw direction (in an arc)
        Vector2 direction = (playerController.transform.position - throwPoint.position).normalized;

        // Apply the throw force (add an arc effect)
        float xForce = direction.x * throwForce; // Horizontal force
        float yForce = Mathf.Abs(direction.y + 0.5f) * throwForce; // Vertical force, adds arc
        rb.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);
    }

    // Flip direction when patrolling between points
    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public override void UseAbility()
    {
        // Not implemented for this enemy
    }

    public override void Die()
    {
    
        if (playerController != null)
        {
            playerController.GainAbilitiesFromEnemy(this); 
        }
        cdTimer.timeRemaining = newTimeRemaining;
        gameObject.SetActive(false);
    }
}
