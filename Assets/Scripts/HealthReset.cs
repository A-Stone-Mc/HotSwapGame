using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReset : MonoBehaviour
{
   
    private HealthUIManager healthUIManager;
    private PlayerController playerController; 
   

    private void Start()
    {
        
        healthUIManager = FindObjectOfType<HealthUIManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            playerController.ResetHealthToMax();
            
            
        }
    }
}
