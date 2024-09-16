using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public int damage = 1;
     private Collider2D lightningCollider;
    void Start()
    {
       
        lightningCollider = GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Deal damage to player
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1); // 
            }
            Destroy(gameObject); 
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject); 
        }

        if (collision.CompareTag("Enemy"))
        {
           lightningCollider.enabled = false;
        }
    }
}
