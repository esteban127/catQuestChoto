using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour {
    
   
    [SerializeField] float respawnTime = 1;
    [SerializeField] float spawnRange = 3;
    [SerializeField] string poolNameToUse;    
    private float currentRespawnTime;
    private PoolManager myPoolManager;

    private void Start()
    {
        myPoolManager = PoolManager.Instance;
    }


    void Update () {
        if (!myPoolManager.PoolIsFull(poolNameToUse))
        {
            if (currentRespawnTime<=0)
            {
                currentRespawnTime = respawnTime;
                Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRange, spawnRange), transform.position.y, transform.position.z + Random.Range(-spawnRange, spawnRange));
                myPoolManager.RequestToPool(poolNameToUse,spawnPosition, new Vector3(0, Random.Range(0, 360), 0), transform.localScale);                
            }
            currentRespawnTime -= Time.deltaTime;
        }
        
	}
}
