using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MonoBehaviour
{
    private PlayerController pC;
    private bool isLiteMode;

       void Start()
    {
        isLiteMode = PlayerPrefs.GetString("Difficulty") == "Lite";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            pC = collision.GetComponent<PlayerController>();

            if (pC != null)
            {
                if (isLiteMode)
                {
                    pC.flySpeed = 3.0f;
                }
                else
                {
                    pC.flySpeed = 5.0f; 
                }
            }   

        }
    }
}
