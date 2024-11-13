using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBarController : MonoBehaviour
{
    public Image healthFill;  
    public Canvas healthBarCanvas;

    private void Start()
    {
        healthBarCanvas = GetComponentInParent<Canvas>();
        healthBarCanvas.enabled = false; 
    }

    public void SetHealth(float healthPercentage)
    {
        healthFill.fillAmount = healthPercentage; 
    }

    public void ActivateHealthBar()
    {
        healthBarCanvas.enabled = true; 
    }

    public void DeactivateHealthBar()
    {
        healthBarCanvas.enabled = false;  
    }
}
