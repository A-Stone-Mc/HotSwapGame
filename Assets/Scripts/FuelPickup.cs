using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    public float fuelAmount = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            {
                FuelManager.Instance.AddFuel(fuelAmount);
                Destroy(gameObject);
            }
        }
    }
}
