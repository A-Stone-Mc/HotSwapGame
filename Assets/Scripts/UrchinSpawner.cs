using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnInterval = 2f; 
    public Vector3 spawnOffset; 

    private void Start()
    {
        
        StartCoroutine(SpawnPrefabRoutine());
    }

    private IEnumerator SpawnPrefabRoutine()
    {
        while (true) 
        {
            SpawnPrefab();
            yield return new WaitForSeconds(spawnInterval); 
        }
    }

    private void SpawnPrefab()
    {
        
        Instantiate(prefabToSpawn, transform.position + spawnOffset, Quaternion.identity);
    }
}
