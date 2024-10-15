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
    public Transform hammer;  // The hammer 
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
            StartCoroutine(HammerSmash());
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
        transform.position += direction * moveSpeed * Time.deltaTime;

        
        if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
        {
            Flip();
        }

        
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    
    private IEnumerator HammerSmash()
    {
        isSmashing = true;

       
        if (animator != null)
        {
            animator.SetTrigger("RaiseHammer");
        }

        yield return new WaitForSeconds(1f);  

        
        if (animator != null)
        {
            animator.SetTrigger("Smash");
        }

        
        yield return new WaitForSeconds(0.5f);

        
        if (smashEffect != null)
        {
            Instantiate(smashEffect, hammerSmashPoint.position, Quaternion.identity);
        }

        if (smashSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(smashSound);
        }

        
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

    
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        // Flip the sprite
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        // Flip the hammer
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

    public override void UseAbility()
    {
       
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
