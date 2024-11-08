using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReset : MonoBehaviour
{
   
    private HealthUIManager healthUIManager;
    private PlayerController playerController; 
    private AudioSource audioSource;
    public AudioClip resetSound;
   

    private void Start()
    {
        
        healthUIManager = FindObjectOfType<HealthUIManager>();
        playerController = FindObjectOfType<PlayerController>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(resetSound);
            playerController.ResetHealthToMax();
            
            
        }
    }
}
