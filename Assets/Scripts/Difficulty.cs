using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    void Start()
    {
        // Set default difficulty to Normal
        if (!PlayerPrefs.HasKey("Difficulty"))
        {
            PlayerPrefs.SetString("Difficulty", "Normal");
        }
    }

    public void SetNormalMode()
    {
        PlayerPrefs.SetString("Difficulty", "Normal");
    }

    public void SetLiteMode()
    {
        PlayerPrefs.SetString("Difficulty", "Lite");
    }
}
