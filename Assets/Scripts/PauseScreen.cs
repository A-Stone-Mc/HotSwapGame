using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseMenuPanel;  
    public GameObject howToPlayPanel;  
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
        pauseMenuPanel.SetActive(true);  
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;  
        isPaused = false;
        pauseMenuPanel.SetActive(false);  
        howToPlayPanel.SetActive(false);  
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;  
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
    }

    public void ShowHowToPlay()
    {
        pauseMenuPanel.SetActive(false);  
        howToPlayPanel.SetActive(true);  
    }

    public void BackToPauseMenu()
    {
        howToPlayPanel.SetActive(false);  
        pauseMenuPanel.SetActive(true);   
    }
}