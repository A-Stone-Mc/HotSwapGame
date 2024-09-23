using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCounter : MonoBehaviour
{
    public TMP_Text timerText;
    private float levelTime = 0f;
    private bool levelCompleted = false;
    public GameObject endPopup;
    public TMP_Text finalTimeText;

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
       
        Debug.Log("Level Completed! Time: " + timerText.text);
    }
}
