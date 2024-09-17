using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float speed = 10f;           // Speed of the laser beam
    public float lifetime = 2f;         // Time after which the laser beam is destroyed
    public int damage = 1;              // Damage dealt by the laser beam
    private Vector2 direction;          // Direction the laser is moving in

    private void Start()
    {
        // Destroy the laser after a certain amount of time to avoid clutter in the scene
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the laser in the specified direction
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Set the direction of the laser
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the laser hits the player
        if (collision.CompareTag("Player"))
        {
            // Deal damage to the player
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // Destroy the laser after it hits the player
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {
            // Destroy the laser if it hits the ground or a wall
            Destroy(gameObject);
        }
    }
}
