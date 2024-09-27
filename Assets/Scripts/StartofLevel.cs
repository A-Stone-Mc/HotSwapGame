using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartofLevel : MonoBehaviour
{
    public GameObject instructionPanel; 
    public Button goButton; 
    public bool alwaysShowInstructions = false;

    void Start()
    {
        
        bool hasSeenInstructions = PlayerPrefs.GetInt("HasSeenInstructions", 0) == 1;

        if (!hasSeenInstructions || alwaysShowInstructions)
        {
            
            Time.timeScale = 0f;
            instructionPanel.SetActive(true);

            
            goButton.onClick.AddListener(StartLevel);
        }
        else
        {
            
            StartLevel();
        }
    }

    void StartLevel()
    {
        
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);
        }

        
        Time.timeScale = 1f;

        
        if (!alwaysShowInstructions)
        {
            PlayerPrefs.SetInt("HasSeenInstructions", 1);
        }
    }
}
