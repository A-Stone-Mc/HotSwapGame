using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformer : MonoBehaviour
{
    private PlatformEffector2D effector; 
    public float waitTime = 0.5f; // Delay before the platform becomes solid

    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {  
            effector.rotationalOffset = 180f;
            StartCoroutine(ResetEffector());

        }
    }

   
    IEnumerator ResetEffector()
    {
        yield return new WaitForSeconds(waitTime);
        effector.rotationalOffset = 0f;
    }
}
