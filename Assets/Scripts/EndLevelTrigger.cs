using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    public LevelCounter levelTimer; 
    public CDTimer countdownTimer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            levelTimer.OnLevelCompleted();
            countdownTimer.DeactivateCountdown(); 
        }
    }
}
