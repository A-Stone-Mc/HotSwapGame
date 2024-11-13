using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformer : MonoBehaviour
{
    private Collider2D platformCollider;
    public float waitTime = 0.5f; // Delay before the platform becomes solid again

    private void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Check if player is pressing the down key to drop through
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(DisableColliderTemporarily());
        }
    }

    private IEnumerator DisableColliderTemporarily()
    {
        platformCollider.enabled = false; // Disable the platform's collider
        yield return new WaitForSeconds(waitTime); // Wait for a short time
        platformCollider.enabled = true; // Re-enable the collider
    }
}
