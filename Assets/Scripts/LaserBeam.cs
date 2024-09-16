using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    private float maxDistance;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the laser forward
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Check if the laser has traveled its maximum distance
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void SetDistance(float distance)
    {
        maxDistance = distance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the laser hits the player
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        // Destroy laser when it hits any obstacle
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
