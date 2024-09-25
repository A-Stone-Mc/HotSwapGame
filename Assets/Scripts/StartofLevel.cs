using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartofLevel : MonoBehaviour
{
    public GameObject instructionPanel; 
    public Button goButton; 

    void Start()
    {
        
        Time.timeScale = 0f;

        
        instructionPanel.SetActive(true);

        
        goButton.onClick.AddListener(StartLevel);
    }

    void StartLevel()
    {
        
        instructionPanel.SetActive(false);

        
        Time.timeScale = 1f;
    }
}