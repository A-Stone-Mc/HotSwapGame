using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); 
    }

    public void OpenLevelSelect()
    {
        levelSelectPanel.SetActive(true);
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene("Level" + levelNumber); 
    }
}