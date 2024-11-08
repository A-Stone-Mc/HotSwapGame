using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayingEnemyController : EnemyController
{
    public GameObject deathEffectPrefab;
    public float effectDuration = 1f;    
    public Vector3 deathEffectScale = new Vector3(0.1f, 0.1f, 0.1f); 
    public Vector3 deathEffectOffset = new Vector3(0, 0.5f, 0);
    public Transform pointA;
    public Transform pointB;
    public GameObject sprayObjectPrefab;
    public GameObject shieldPrefab;
    public float sprayInterval = 5f;
    public float shieldDuration = 3f;
    public float waitAfterShield = 2f;
    public float detectionRange = 5f;
    public int numberOfSprayObjects = 5;
    public float delayBetweenParticles = 0.2f;  // Delay between each particle

    private Transform player;
    private bool isSpraying = false;
    private bool isInvincible = false;
    private bool movingToB = true;
    private GameObject activeShield;
    private Animator animator; 
    public float initialSprayDelay = 1.5f;  
    public Transform sprayPoint; 

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();  // Get the Animator component
        StartCoroutine(SprayRoutine());
    }

    private void Update()
    {
        if (!isSpraying && !isInvincible)
        {
            Patrol();
            animator.SetBool("IsMoving", true); 
        }
        else
        {
            animator.SetBool("IsMoving", false);  
        }

        DetectPlayer();
    }

    private void Patrol()
    {
        
        if (movingToB)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
            {
                movingToB = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, pointA.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
            {
                movingToB = true;
            }
        }
    }

    private void DetectPlayer()
    {
        
        if (Vector2.Distance(transform.position, player.position) < detectionRange && !isSpraying && !isInvincible)
        {
            StopAllCoroutines();
            StartCoroutine(SprayRoutine());
        }
    }

    private IEnumerator SprayRoutine()
    {
        isSpraying = true;
        animator.SetTrigger("Spray");
        
        yield return new WaitForSeconds(initialSprayDelay);

        
        for (int i = 0; i < numberOfSprayObjects; i++)
        {
            Vector3 spawnPosition = sprayPoint ? sprayPoint.position : transform.position;
            GameObject sprayObject = Instantiate(sprayObjectPrefab, spawnPosition, Quaternion.identity);
            sprayObject.GetComponent<FollowPlayer>().Initialize(player); 
            yield return new WaitForSeconds(delayBetweenParticles);
        }

        yield return new WaitForSeconds(1f);
        ActivateShield();

        yield return new WaitForSeconds(shieldDuration);
        DeactivateShield();

        isSpraying = false;
        yield return new WaitForSeconds(waitAfterShield); 
    }

    private void ActivateShield()
    {
        canTakeDamage = false;
        isInvincible = true;
        activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        activeShield.transform.parent = transform; 
        activeShield.transform.localPosition = Vector3.zero; 

        
        var shieldRenderer = activeShield.GetComponent<SpriteRenderer>();
        if (shieldRenderer != null)
        {
            shieldRenderer.sortingOrder = spriteRenderer.sortingOrder + 1; 
        }
    }

    private void DeactivateShield()
    {
        canTakeDamage = true;
        isInvincible = false;
        if (activeShield != null)
        {
            Destroy(activeShield); // Destroy shield
        }
    }

    public override void Die()
    {
        SpawnDeathEffect();
        Destroy(gameObject); 
    }

    public override void UseAbility()
    {
        
    }

    public new void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        
        if (!isInvincible)
        {
            base.TakeDamage(damage, knockbackDirection);
        }
    }

    private void SpawnDeathEffect()
    {
        
        GameObject effect = Instantiate(deathEffectPrefab, transform.position + deathEffectOffset, Quaternion.identity);

       
        effect.transform.localScale = deathEffectScale; 

        Debug.Log("Death effect instantiated and positioned over the enemy.");

        
        Destroy(effect, effectDuration);
    }
}
