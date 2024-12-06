using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public AudioClip restartSound; 
    public float restartDelay = 1.5f; 
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(RestartLevelWithDelay());
        }
    }

    private IEnumerator RestartLevelWithDelay()
    {
        if (audioSource != null && restartSound != null)
        {
            audioSource.PlayOneShot(restartSound); 
        }

        yield return new WaitForSeconds(restartDelay); 

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}

