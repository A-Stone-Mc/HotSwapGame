using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyController : EnemyController
{
    public Transform pointA; // Patrol point A
    public Transform pointB; // Patrol point B
    public GameObject laserBeamPrefab; // The laser beam prefab
    public Transform laserFirePoint; // The point from which the laser is fired
    public float laserRange = 10f; // How far the laser can reach
    public float laserCooldown = 3f; // Time between laser shots
    public float detectionRange = 8f; // How close the player must be 
    private float laserCooldownTimer;
    private Vector3 targetPosition;
    private bool playerInRange = false;
    public float waitTime = 2f;   // Time to wait at each patrol point
    private bool isWaiting = false;

    private PlayerController playerController;
    public AudioSource laserAudioSource;
    public AudioClip laserSoundEffect;

    private Animator animator;

    private void Start()
    {
        targetPosition = pointB.position; 
        laserCooldownTimer = laserCooldown;
        if (laserAudioSource == null)
        {
            laserAudioSource = GetComponent<AudioSource>();
        }

        animator = GetComponent<Animator>();

        
        if (animator != null)
        {
            animator.Play("Patrol");
        }
    }

    private void Update()
    {
        Patrol();
        CheckPlayerInRange();
        if (playerInRange && laserCooldownTimer <= 0)
        {
            FireLaser();
            laserCooldownTimer = laserCooldown;
        }
        laserCooldownTimer -= Time.deltaTime;
        UpdatePlayerReference();
    }

    private void UpdatePlayerReference()
    {
        if (playerController == null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("PlayerController found and set.");
            }
            else
            {
                Debug.LogError("PlayerController is still null. Check if the Player has the 'Player' tag.");
            }
        }
    }

    // Patrol logic between two points
    private void Patrol()
    {
        if (!playerInRange && !isWaiting)  // Only patrol if not waiting and player is not in range
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Check if the enemy reached the patrol point
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                StartCoroutine(WaitAndSwitchDirection());  // Wait before switching direction
            }

            // Play patrol animation if applicable
            if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Patrol"))
            {
                animator.Play("Patrol");
            }
        }
    }


    private IEnumerator WaitAndSwitchDirection()
    {
        isWaiting = true;  

        
        yield return new WaitForSeconds(waitTime);

        
        targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;

        Flip();  

        isWaiting = false;  
    }

    
    private void CheckPlayerInRange()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectionRange)
            {
                playerInRange = true;
                FacePlayer(player);
            }
            else
            {
                playerInRange = false;
            }
        }
    }

    
    private void FacePlayer(GameObject player)
    {
        Vector3 direction = player.transform.position - transform.position;
        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    // Fire the laser beam in the direction the enemy is facing
    private void FireLaser()
    {
        Debug.Log("Laser Fired!");
        if (laserSoundEffect != null && laserAudioSource != null)
        {
            laserAudioSource.PlayOneShot(laserSoundEffect);
        }
        else
        {
            Debug.LogWarning("no laser sound effect");
        }

        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        // Instantiate the laser beam
        GameObject laserBeam = Instantiate(laserBeamPrefab, laserFirePoint.position, laserFirePoint.rotation);

        // Determine the direction based on the enemy's facing direction
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Pass the direction to the laser beam
        LaserBeam beamScript = laserBeam.GetComponent<LaserBeam>();
        if (beamScript != null)
        {
            beamScript.SetDirection(direction);  // Set the direction to left or right
        }

        // Flip the laser if the enemy is facing left (this only flips the sprite, not the movement)
        if (direction == Vector2.left)
        {
            laserBeam.transform.localScale = new Vector3(-laserBeam.transform.localScale.x, laserBeam.transform.localScale.y, laserBeam.transform.localScale.z);
        }
    }

    // Flip direction
    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public override void UseAbility()
    {
        Debug.Log("Laser Enemy's ability activated!");
    }

    public override void Die()
    {
        if (playerController != null)
        {
            Debug.Log("Player is gaining laser abilities");
            playerController.GainAbilitiesFromEnemy(this);  
        }
        cdTimer.timeRemaining = newTimeRemaining;
        gameObject.SetActive(false);

    }

}