using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseText; 
    private bool isPaused = false;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        
        Time.timeScale = 0f;
        isPaused = true;

        // Show the pause text
        pauseText.SetActive(true);
    }

    void ResumeGame()
    {
        
        Time.timeScale = 1f;
        isPaused = false;

        
        pauseText.SetActive(false);
    }
}