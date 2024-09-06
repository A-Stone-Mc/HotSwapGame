using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public int damage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the bomb
        }
        else
        {
            
            Destroy(gameObject); 
        }
    }
}
