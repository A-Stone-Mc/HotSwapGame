using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FlyingEnemyController : EnemyController
{
    public Transform pointA; // The left point 
    public Transform pointB; // The right point 
    public float bobbingAmplitude = 0.5f; // The height of the bobbing 
    public float bobbingFrequency = 1f;   // The speed of the bobbing

    public GameObject bombPrefab;
    public Transform dropPoint1;  
    public Transform dropPoint2;  
    public float dropInterval = 3f; // Time between drops
    private float dropTimer;

    private Vector3 targetPosition;
    private PlayerController playerController;
    public AudioSource bombAudioSource; 
    public AudioClip bombDropSoundEffect;
    private Renderer enemyRenderer;
    private Animator animator;
    

    void Start()
    {
        dropTimer = dropInterval;
        targetPosition = pointB.position; // Start by moving towards point B
        if (bombAudioSource == null)
        {
            bombAudioSource = GetComponent<AudioSource>();
        }
        enemyRenderer = GetComponent<Renderer>();
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("FlyingEnemyAnimation"); 
        }
    }

    void Update()
    {
        Move();
        DropBombs();
        UpdatePlayerReference();
    }

    private void UpdatePlayerReference()
    {
        if (playerController == null)
        {
            Debug.Log("Trying to find PlayerController...");
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

    // Override the base 
    public override void Move()
    {
        // Move between pointA and pointB
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Switch direction when the enemy reaches a target point
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
        }

        // Bobbing effect for vertical movement (sine wave)
        float newY = Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
        transform.position = new Vector3(transform.position.x, newY + (pointA.position.y + pointB.position.y) / 2, transform.position.z);
    }

    // Drop bombs from both drop points
    void DropBombs()
    {
        dropTimer -= Time.deltaTime;
        if (dropTimer <= 0)
        {
            if (enemyRenderer.isVisible) 
            {

                if (bombDropSoundEffect != null && bombAudioSource != null)
                {
                    bombAudioSource.PlayOneShot(bombDropSoundEffect);
                }
                else
                {
                    Debug.LogWarning("no lightning sound effect");
                }
            }
            
            Instantiate(bombPrefab, dropPoint1.position, Quaternion.identity);
            Instantiate(bombPrefab, dropPoint2.position, Quaternion.identity);

            dropTimer = dropInterval; 
        }
    }

    public override void UseAbility()
    {
        Debug.Log("Flying Enemy's ability activated!");
    }


    public override void Die()
    {
        if (playerController != null)
        {
            Debug.Log("Enemy is calling GainAbilitiesFromEnemy.");
            playerController.GainAbilitiesFromEnemy(this);  
        }
        else
        {
            Debug.LogError("PlayerController is null in EnemyController.");
        }
        cdTimer.timeRemaining = newTimeRemaining;
        gameObject.SetActive(false);

    }



}

