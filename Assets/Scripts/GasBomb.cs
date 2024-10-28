using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBomb : MonoBehaviour
{
    public GameObject gasCloudPrefab; 
    public float gasCloudDuration = 5f; 
    public LayerMask groundLayer; 
    private Rigidbody2D rb;
    private bool hasDetonated = false;
    public AudioClip gasCloudSound;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        
        rb.gravityScale = 1.0f;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gasCloudSound;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player")) && !hasDetonated)
        {
            hasDetonated = true; 
            SpawnGasCloud();
        }
    }

    private void SpawnGasCloud()
    {
        
        GameObject gasCloud = Instantiate(gasCloudPrefab, transform.position, Quaternion.identity);
        
        
       
        Destroy(gameObject);

        audioSource.Play();
        
        Destroy(gasCloud, gasCloudDuration);
    }
}
