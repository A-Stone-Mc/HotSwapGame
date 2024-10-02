using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelCounter : MonoBehaviour
{
    public TMP_Text timerText;
    private float levelTime = 0f;
    private bool levelCompleted = false;
    public GameObject endPopup;
    public TMP_Text finalTimeText;
    public TMP_Text fuelStatusText;

    public Button continueButton;
    public Button retryButton;

    void Update()
    {
        if (!levelCompleted)
        {
            levelTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        
        int minutes = Mathf.FloorToInt(levelTime / 60F);
        int seconds = Mathf.FloorToInt(levelTime % 60F);
       
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds );
    }

    public void OnLevelCompleted()
    {
        levelCompleted = true;
        ShowEndPopup();
    }

    void ShowEndPopup()
    {
        
        endPopup.SetActive(true);
        finalTimeText.text = "Your Time: " + timerText.text;
        if (FuelManager.Instance.FuelIsEnough())
        {
            fuelStatusText.text = "Congrats! You collected enough fuel.";
            
            continueButton.gameObject.SetActive(true);
            retryButton.gameObject.SetActive(false);

            continueButton.onClick.AddListener(() => LoadNextLevel());
        }
        else
        {
            fuelStatusText.text = "Oh no, you didn't collect enough fuel for your tank!";
            continueButton.gameObject.SetActive(false);
        }

        retryButton.gameObject.SetActive(true);

        retryButton.onClick.AddListener(() => RetryLevel());
       
        Debug.Log("Level Completed! Your Time: " + timerText.text);
    }


    public void LoadNextLevel()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // CHANGE LOADING NEXT SCENE
    }

    public void RetryLevel()
    {
        // Restart the current level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
