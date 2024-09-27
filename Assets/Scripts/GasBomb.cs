using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBomb : MonoBehaviour
{
    public GameObject gasCloudPrefab; // The gas cloud prefab
    public float gasCloudDuration = 5f; // How long the gas cloud will stay
    public LayerMask groundLayer; // Layer for the ground, to detect when the bomb hits it
    private Rigidbody2D rb;
    private bool hasDetonated = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure gravity is enabled so the bomb falls in an arc
        rb.gravityScale = 1.0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bomb hits the ground or player, and detonate
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player")) && !hasDetonated)
        {
            hasDetonated = true; // Prevent multiple detonations
            SpawnGasCloud();
        }
    }

    private void SpawnGasCloud()
    {
        // Instantiate the gas cloud at the current position
        GameObject gasCloud = Instantiate(gasCloudPrefab, transform.position, Quaternion.identity);

        // Destroy the gas bomb
        Destroy(gameObject);

        // Destroy the gas cloud after a certain time
        Destroy(gasCloud, gasCloudDuration);
    }
}
