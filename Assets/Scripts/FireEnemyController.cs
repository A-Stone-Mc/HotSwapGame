using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyController : EnemyController
{

    public GameObject deathEffectPrefab; 
    public float effectDuration = 1f;    
    public Vector3 deathEffectScale = new Vector3(0.1f, 0.1f, 0.1f); 
    public Vector3 deathEffectOffset = new Vector3(0, 0.5f, 0); 
    public Transform pointA; // Patrol point A
    public Transform pointB; // Patrol point B
    public Transform firePoint;
    public float detectionRange = 8f; 
    public float fireDuration = 5f; 
    public float fireCooldown = 3f; 
    public float fireDamageInterval = 2f;
    public float fireDamageRadius = 2f;

    private bool playerInRange = false;
    private bool isFiring = false;
    private float fireCooldownTimer = 0f;
    private Animator animator;
    private bool isFacingRight = true;
    public GameObject rightFireStreamPrefab;
    public GameObject leftFireStreamPrefab;

    public Transform player; 
    private Vector3 targetPosition;

    private PlayerController playerController;
    private GameObject activeFireStream;

    private void Start()
    {
        animator = GetComponent<Animator>();
        targetPosition = pointB.position; 
    }

    private void Update()
    {
        Patrol();
        CheckPlayerInRange();

       
        if (fireCooldownTimer > 0)
        {
            fireCooldownTimer -= Time.deltaTime;
        }

        if (playerInRange && fireCooldownTimer <= 0 && !isFiring)
        {
           
            animator.SetTrigger("IsFiring");
        }

        UpdateAnimations(); 
    }

   
    private void Patrol()
    {
        if (!playerInRange && !isFiring)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
                Flip();
            }
        }
    }

    private void CheckPlayerInRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        playerInRange = distanceToPlayer <= detectionRange;

        if (playerInRange)
        {
            FacePlayer(player);
        }
    }

    
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

    
    private IEnumerator ShootFire()
    {
        isFiring = true;

        
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

       
        Destroy(activeFireStream);

        fireCooldownTimer = fireCooldown;  
        isFiring = false;
    }

    
    private IEnumerator DamagePlayerOverTime()
    {
        while (isFiring)
        {
           
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

            yield return new WaitForSeconds(fireDamageInterval);  
        }
    }

    
    public void OnFireAnimationEvent()
    {
        StartCoroutine(ShootFire());
    }

   
    private void UpdateAnimations()
    {
       
        if (isFiring) return;

        
        bool isWalking = Vector3.Distance(transform.position, targetPosition) > 0.1f;
        animator.SetBool("IsWalking", isWalking);
    }

    
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }


    public override void UseAbility() {}

    public override void Die()
    {
        Destroy(activeFireStream);
        if (playerController != null)
        {
            playerController.GainAbilitiesFromEnemy(this);
        }

        SpawnDeathEffect();
        cdTimer.timeRemaining = newTimeRemaining;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, fireDamageRadius);
    }

    private void SpawnDeathEffect()
    {
        
        GameObject effect = Instantiate(deathEffectPrefab, transform.position + deathEffectOffset, Quaternion.identity);

       
        effect.transform.localScale = deathEffectScale; 

        Debug.Log("Death effect instantiated and positioned over the enemy.");

        
        Destroy(effect, effectDuration);
    }

}
