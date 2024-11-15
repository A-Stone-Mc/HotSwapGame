using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;
    private float followSpeed = 3f;
    public int damage = 1;
    private float bobbingAmplitude = 10f;
    private float bobbingFrequency = 4f; 
    private float initialY;

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        initialY = transform.position.y;
    }

    private void Update()
    {
        if (player != null)
        {
            float bobbingOffset = Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
            Vector2 targetPosition = new Vector2(player.position.x, player.position.y + bobbingOffset);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
                Destroy(gameObject); // Destroy after hitting the player
            }
        }
    }
}