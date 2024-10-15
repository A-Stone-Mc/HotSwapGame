using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasEnemyController : EnemyController
{
    public Transform pointA;
    public Transform pointB; 
    public Transform throwPoint; 
    public GameObject gasBombPrefab; 
    public float throwForce = 10f;
    public float detectionRange = 10f; 
    public float attackCooldown = 5f; 
    public float PatrolmoveSpeed = 2f;
    private float attackCooldownTimer;

    private Vector3 targetPosition;
    private bool playerInRange = false;
    private PlayerController playerController;
    private bool isAttacking = false;

    private Animator animator; 

    private void Start()
    {
        targetPosition = pointB.position;
        attackCooldownTimer = attackCooldown;
        animator = GetComponent<Animator>(); 
    }

    private void Update()
    {
        Patrol();
        CheckPlayerInRange();
        attackCooldownTimer -= Time.deltaTime; 
        
       
        
        if (playerInRange && attackCooldownTimer <= 0 && !isAttacking)
        {
            
            
            isAttacking = true;
            animator.SetTrigger("Throw");  
            
            attackCooldownTimer = attackCooldown; 
        }

        UpdatePlayerReference();
    }


    private void UpdatePlayerReference()
    {
        if (playerController == null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        }
    }

    private void Patrol()
    {
        if (!playerInRange)
        {
            animator.SetBool("IsWalking", true); 

            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
                Flip();
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

 
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

    public void ThrowGasBomb()
    {
        if (playerController == null) return;
        
        GameObject gasBomb = Instantiate(gasBombPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = gasBomb.GetComponent<Rigidbody2D>();

        
        Vector2 direction = (playerController.transform.position - throwPoint.position).normalized;
        float xForce = direction.x * throwForce; 
        float yForce = Mathf.Abs(direction.y + 0.5f) * throwForce; 
        rb.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);

        
       
    
        StartCoroutine(CooldownAfterThrow());
    }

    private IEnumerator CooldownAfterThrow()
    {
        
        yield return new WaitForSeconds(attackCooldown);

        
        isAttacking = false;
 
    }

    
    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
