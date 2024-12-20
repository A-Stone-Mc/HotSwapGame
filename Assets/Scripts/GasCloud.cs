using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GasCloud : MonoBehaviour
{
    public int damage = 1; 
    public float damageInterval = 2f; 
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Start damaging the player
                StartCoroutine(DamagePlayer(player));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop damaging the player when they leave the cloud
            StopAllCoroutines();
        }
    }

    private IEnumerator DamagePlayer(PlayerController player)
    {
        while (true)
        {
            // Deal damage to the player every 2 seconds
            player.TakeGasDamage(damage);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
