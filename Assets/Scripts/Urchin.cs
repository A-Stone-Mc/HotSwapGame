using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urchin : MonoBehaviour
{
    public int damage = 1;
    private Collider2D lightningCollider;
    public float destroyDelay = 2.0f; 

    void Start()
    {
        lightningCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground") || collision.CompareTag("Bounds"))
        {
            StartCoroutine(DestroyAfterDelay());
        }
        else if (collision.CompareTag("Enemy"))
        {
            lightningCollider.enabled = false; 
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay); 
        Destroy(gameObject); 
    }
}
