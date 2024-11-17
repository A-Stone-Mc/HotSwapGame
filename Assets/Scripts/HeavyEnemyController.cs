using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemyController : EnemyController
{
    public Transform player;  
    public float detectionRange = 10f;  
    public float NewmoveSpeed = 2f;  
    public float hammerSmashCooldown = 5f;  
    public float hammerSmashDamageRadius = 3f; 
    public int hammerSmashDamage = 2; 
    public Transform hammer;  
    public Transform hammerSmashPoint;  
    public ParticleSystem smashEffect;  
    public AudioClip smashSound; 
    private bool playerInRange = false;
    private bool isSmashing = false;
    private float smashCooldownTimer = 0f;

    private Animator animator; 
    private AudioSource audioSource;  
    private bool isFacingRight = true; 
    private PlayerController playerController;
    public GameObject deathEffectPrefab;
    public float effectDuration = 1f;    
    public Vector3 deathEffectScale = new Vector3(0.1f, 0.1f, 0.1f); 
    public Vector3 deathEffectOffset = new Vector3(0, 0.5f, 0);

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckPlayerInRange();

        if (playerInRange && !isSmashing)
        {
            MoveTowardsPlayer();
        }

        
        if (smashCooldownTimer > 0)
        {
            smashCooldownTimer -= Time.deltaTime;
        }
        else if (playerInRange && !isSmashing)
        {
            
            StartHammerSmash();
        }
    }

   
    private void CheckPlayerInRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        playerInRange = distanceToPlayer <= detectionRange;
    }

    
    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 horizontalMove = new Vector3(direction.x, 0f, 0f);
        transform.position += horizontalMove * NewmoveSpeed * Time.deltaTime;

        if ((horizontalMove.x > 0 && !isFacingRight) || (horizontalMove.x < 0 && isFacingRight))
        {
            Flip();
        }

        animator.SetBool("IsWalking", horizontalMove.magnitude > 0.1f);
    }

    // Called by the animation event
    public void HammerSmash()
    {
        Debug.Log("Hammer Smash Triggered!");

        if (smashEffect != null)
        {
            Instantiate(smashEffect, hammerSmashPoint.position, Quaternion.identity);
        }

        if (smashSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(smashSound);
        }

        // Deal damage to player(s) in radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(hammerSmashPoint.position, hammerSmashDamageRadius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerController playerController = hitCollider.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(hammerSmashDamage);
                }
            }
        }

        smashCooldownTimer = hammerSmashCooldown;  
        isSmashing = false;  
    }

   
    private void StartHammerSmash()
    {
        if (!isSmashing)
        {
            isSmashing = true;  
            animator.SetTrigger("IsSlamming");  
        }
    }

    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if (hammer != null)
        {
            hammer.localScale = new Vector3(-hammer.localScale.x, hammer.localScale.y, hammer.localScale.z);
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hammerSmashPoint.position, hammerSmashDamageRadius);
    }

    public override void UseAbility() {}

    public override void Die()
    {
        SpawnDeathEffect();
        cdTimer.timeRemaining = newTimeRemaining;
        gameObject.SetActive(false);
    }

    private void SpawnDeathEffect()
    {
        
        GameObject effect = Instantiate(deathEffectPrefab, transform.position + deathEffectOffset, Quaternion.identity);

       
        effect.transform.localScale = deathEffectScale; 

        
        Destroy(effect, effectDuration);
    }
}
