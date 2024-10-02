using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CDTimer : MonoBehaviour
{
    public float timeRemaining = 10f;
    public TMP_Text timerText;
    public bool isCountdownActive = true;

    void Update()
    {
        if (isCountdownActive && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = timeRemaining.ToString("F2");
        }
        else if (timeRemaining <= 0)
        {
            RestartLevel();
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
