using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    public int maxHealth = 4; 
    public int currentHealth; 
    public List<GameObject> heartImages; 

    private void Start()
    {
        
        currentHealth = maxHealth;
        UpdateHearts(); 
    }

    
    public void UpdateHearts()
    {
        
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].SetActive(true);  // Enable heart if within current health
            }
            else
            {
                heartImages[i].SetActive(false); // Disable heart if health is lost
            }
        }
    }

    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        
        UpdateHearts();
    }

    
    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        
        UpdateHearts();
    }
}
