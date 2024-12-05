using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class CreditsManager : MonoBehaviour
{
    public RectTransform creditsText; 
    public float scrollSpeed = 30f;

    void Update()
    {
        
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}