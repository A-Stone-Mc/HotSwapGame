using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackSpeed : MonoBehaviour
{
    private PlayerController pC;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            pC = collision.GetComponent<PlayerController>();

            if (pC != null)
            {
                pC.flySpeed = 2.0f;
            }
            else
            {
                Debug.LogWarning("PlayerControllernot found");
            }
        }
    }
}
