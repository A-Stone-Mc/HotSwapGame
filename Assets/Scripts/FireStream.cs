using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStream : MonoBehaviour
{
    private Transform target;
    private float damageInterval;
    private bool isDealingDamage = false;

    public void StartFireStream(Transform playerTarget, float interval)
    {
        target = playerTarget;
        damageInterval = interval;
        isDealingDamage = true;
        StartCoroutine(DealDamage());
    }

    private IEnumerator DealDamage()
    {
        while (isDealingDamage)
        {
            if (Vector2.Distance(transform.position, target.position) < 2f) 
            {
                PlayerController playerController = target.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(1); 
                }
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnDestroy()
    {
        isDealingDamage = false;
    }
}