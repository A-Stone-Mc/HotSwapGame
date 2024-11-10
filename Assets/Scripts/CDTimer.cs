using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CDTimer : MonoBehaviour
{
    public float timeRemaining = 10f;
    public TMP_Text timerText;
    public bool isCountdownActive = false;

    private float lifeLossCountdown = 5f; 
    private float damageInterval = 5f; 
    private bool isLiteMode;
    private bool isLifeLossActive = false;
    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        isLiteMode = PlayerPrefs.GetString("Difficulty") == "Lite";
    }

    void Update()
    {
        if (isCountdownActive && timeRemaining > 0)
        {
           
            timeRemaining -= Time.deltaTime;
            timerText.text = timeRemaining.ToString("F2");
        }
        else if (timeRemaining <= 0)
        {
            if (isLiteMode)
            {
               
                HandleLiteModeLifeLoss();
            }
            else
            {
                
                RestartLevel();
            }
        }
    }

    void HandleLiteModeLifeLoss()
    {
        if (!isLifeLossActive)
        {
            isLifeLossActive = true;
            timerText.color = Color.red; 
        }

        
        lifeLossCountdown -= Time.deltaTime;
        timerText.text = lifeLossCountdown.ToString("F2");

        if (lifeLossCountdown <= 0)
        {
            // Take damage after runnning out of time
            playerController.TakeTimeDamage(1);
            lifeLossCountdown = damageInterval; 
        }
    }

    public void DeactivateCountdown()
    {
        isCountdownActive = false;
    }

    void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}