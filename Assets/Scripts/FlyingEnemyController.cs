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

    void Start()
    {
        dropTimer = dropInterval;
        targetPosition = pointB.position; // Start by moving towards point B
    }

    void Update()
    {
        Move();
        DropBombs();
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
            
            Instantiate(bombPrefab, dropPoint1.position, Quaternion.identity);
            Instantiate(bombPrefab, dropPoint2.position, Quaternion.identity);

            dropTimer = dropInterval; 
        }
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

