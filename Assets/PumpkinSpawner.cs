using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinSpawner : MonoBehaviour
{
    public GameObject pumpkinPrefab; 
    public Transform[] spawnPoints; 
    public float spawnInterval = 3f; 
    private List<GameObject> spawnedPumpkins = new List<GameObject>();
    private int maxPumpkins = 4;

    void Start()
    {
        
        StartCoroutine(SpawnPumpkinRoutine());
    }

    private IEnumerator SpawnPumpkinRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            
            spawnedPumpkins.RemoveAll(pumpkin => pumpkin == null);

           
            if (spawnedPumpkins.Count < maxPumpkins)
            {
                SpawnPumpkin();
            }
        }
    }

    private void SpawnPumpkin()
    {
        
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        
        GameObject newPumpkin = Instantiate(pumpkinPrefab, spawnPoint.position, Quaternion.identity);
        spawnedPumpkins.Add(newPumpkin);
    }
}
