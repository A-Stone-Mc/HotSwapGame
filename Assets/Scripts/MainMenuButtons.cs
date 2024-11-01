using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject mainMenuPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); 
    }

    public void OpenLevelSelect()
    {
        levelSelectPanel.SetActive(true);
        mainMenuPanel.SetActive(false); 
    }

    public void GoBackToMainMenu()
    {
        levelSelectPanel.SetActive(false); 
        mainMenuPanel.SetActive(true); 
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene("Level" + levelNumber); 
    }
}