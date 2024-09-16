using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : EnemyController
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public LayerMask playerLayer;
    public GameObject laserPrefab;
    public Transform firePoint;
    public float laserCooldown = 2f;

    private Vector3 targetPosition;
    private bool playerDetected = false;
    private float laserTimer = 0f;

    private void Start()
    {
        targetPosition = pointB.position; // Start by moving towards point B
    }

    private void Update()
    {
        if (!playerDetected)
        {
            Patrol();
            DetectPlayer();
        }
        else
        {
            FireLaser();
        }

        laserTimer -= Time.deltaTime; // Countdown for laser cooldown
    }

    void Patrol()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Flip the enemy when it changes direction
        if (targetPosition == pointA.position && Vector3.Distance(transform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
            Flip();
        }
        else if (targetPosition == pointB.position && Vector3.Distance(transform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
            Flip();
        }
    }

    void Flip()
    {
        // Flip the enemy on the x-axis to face the opposite direction
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void DetectPlayer()
    {
        // Detect the player within a certain range
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, detectionRange, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    void FireLaser()
    {
        if (laserTimer <= 0f)
        {
            Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
            laserTimer = laserCooldown; // Reset the laser cooldown
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a line to visualize detection range
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * detectionRange);
    }


    
    public override void UseAbility()
    {
        Debug.Log("Flying Enemy's ability activated!");
    }

    protected override void Die()
    {
        Debug.Log("Flying Enemy Died!");
        gameObject.SetActive(false);
    }
}
