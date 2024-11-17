using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private AudioSource audioSource;
    public AudioClip deathSound;

    public GameObject LaserenemyPrefab;
    public GameObject FireenemyPrefab; 
    public GameObject spawnEffectPrefab;  
    public Transform spawnPoint1;  
    public Transform spawnPoint2;  
    public float spawnEffectDuration = 2f;
    public GameObject healthPowerUpPrefab;    
    private int shieldActivationCount = 0;    

    private int gateSpawnCount = 0;
    public Transform powerUpSpawnPoint;
    public GameObject trapPrefab;           
    public Transform trapSpawnPoint;     
    public Transform trapSpawnPointSecond;     
    public float trapDropRate = 5f;   

    public GameObject gatePrefab;
    public Transform gateSpawn;

    public GameObject pickupPrefab;            
    public int numberOfPickups = 5;             
    public Vector2[] dropOffsets;   
    public float dropRange = 1f;
    public Image enemyHealthBarFill; 
    private int maxHealth; 
    public GameObject backgroundMusic;

    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();  
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        maxHealth = health;

    }

    private void Update()
    {
        if (enemyHealthBarFill != null)
        {
            enemyHealthBarFill.fillAmount = (float)health / maxHealth;
        }
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
            if (backgroundMusic != null && !backgroundMusic.activeSelf)
            {
                backgroundMusic.SetActive(true);
            }
            if (enemyHealthBarFill != null && !enemyHealthBarFill.transform.parent.gameObject.activeSelf)
            {
                enemyHealthBarFill.transform.parent.gameObject.SetActive(true);
            }
            StopAllCoroutines();
            StartCoroutine(SprayRoutine());
            if (gatePrefab != null)
            {
                if(gateSpawnCount < 1)
                {
                    Instantiate(gatePrefab, gateSpawn.position, Quaternion.Euler(0, 0, -90));
                    gateSpawnCount++;
                }
            }
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
        shieldActivationCount++;
        activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        activeShield.transform.parent = transform; 
        activeShield.transform.localPosition = Vector3.zero; 

        
        var shieldRenderer = activeShield.GetComponent<SpriteRenderer>();
        if (shieldRenderer != null)
        {
            shieldRenderer.sortingOrder = spriteRenderer.sortingOrder + 1; 
        }

        StartCoroutine(SpawnEnemiesWithEffect());

        if (shieldActivationCount >= 2)
        {
            StartCoroutine(DropTraps());
        }

    }

    private IEnumerator SpawnEnemiesWithEffect()
    {
        
        if (spawnEffectPrefab != null && shieldActivationCount !=4)
        {
            GameObject effect1 = Instantiate(spawnEffectPrefab, spawnPoint1.position, Quaternion.identity);
            Destroy(effect1, spawnEffectDuration);  

            GameObject effect2 = Instantiate(spawnEffectPrefab, spawnPoint2.position, Quaternion.identity);
            Destroy(effect2, spawnEffectDuration); 
        }

       
        yield return new WaitForSeconds(spawnEffectDuration);

        if (shieldActivationCount == 4 && healthPowerUpPrefab != null)
        {

            Instantiate(healthPowerUpPrefab, powerUpSpawnPoint.position, Quaternion.identity);

        
            shieldActivationCount = 0;
        }
        else
        {
            Instantiate(LaserenemyPrefab, spawnPoint1.position, Quaternion.identity);
            Instantiate(FireenemyPrefab, spawnPoint2.position, Quaternion.identity);
        }

    }

    private IEnumerator DropTraps()
    {
        while (isInvincible) 
        {
            
            if (trapPrefab != null && trapSpawnPoint != null)
            {
                Instantiate(trapPrefab, trapSpawnPoint.position, Quaternion.identity);
                Instantiate(trapPrefab, trapSpawnPointSecond.position, Quaternion.identity);
            }

            
            yield return new WaitForSeconds(trapDropRate);
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
        DropPickups();
        if (enemyHealthBarFill != null)
        {
            enemyHealthBarFill.transform.parent.gameObject.SetActive(false);
        }
        if (backgroundMusic != null && backgroundMusic.activeSelf)
        {
            backgroundMusic.SetActive(false);
        }
        Destroy(gameObject); 
    }

    private void DropPickups()
    {
        for (int i = 0; i < numberOfPickups; i++)
        {
            
            Vector2 randomOffset = new Vector2(
                Random.Range(-dropRange, dropRange),
                Random.Range(-dropRange, dropRange)
            );

            
            Vector2 dropPosition = (Vector2)transform.position + randomOffset;

            
            Instantiate(pickupPrefab, dropPosition, Quaternion.identity);
        }
    }

    public override void UseAbility()
    {
        
    }

    public new void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        
        if (!isInvincible)
        {
            base.TakeDamage(damage, knockbackDirection);
            if (enemyHealthBarFill != null)
            {
                enemyHealthBarFill.fillAmount = (float)health / maxHealth;
               
            }
        }
        
    }

    private void SpawnDeathEffect()
    {
        
        GameObject effect = Instantiate(deathEffectPrefab, transform.position + deathEffectOffset, Quaternion.identity);

       
        effect.transform.localScale = deathEffectScale; 

        
        Destroy(effect, effectDuration);
    }
}
