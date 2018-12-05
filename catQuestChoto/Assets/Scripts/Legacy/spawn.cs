
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct spawnNumber
{
    public GameObject prefabToSpawn;
    public int numberOfSpawns;
    public int enemyLvl;
}


public class spawn : MonoBehaviour {
    
   
    [SerializeField] float respawnTime = 1;
    [SerializeField] float spawnRange = 3;
    [SerializeField] spawnNumber[] spawns;
    [SerializeField] string spawnerID;
    private float currentRespawnTime;
    private PoolManager myPoolManager;
    private GameObject player;
    bool active = false;

    private void Start()
    {
        myPoolManager = PoolManager.Instance;
        for (int i = 0; i < spawns.Length; i++)
        {
                myPoolManager.AddPool(spawns[i].prefabToSpawn, spawns[i].numberOfSpawns, spawnerID + i, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.gameObject;
            active = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            active = false;
        }
    }


    void Update ()
    {
        
        if (active)
        {            
            if(TryToSpawn())
                currentRespawnTime -= Time.deltaTime;
        }
        
    }
    

    private bool TryToSpawn()
    {
        for (int i = 0; i < spawns.Length; i++)
        {
            if (spawns[i].prefabToSpawn != null && spawns[i].numberOfSpawns > 0)
            {
                if (!myPoolManager.PoolIsFull(spawnerID + i))
                {
                    if (currentRespawnTime <= 0)
                    {
                        currentRespawnTime = respawnTime;                        
                        Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRange, spawnRange), transform.position.y, transform.position.z + Random.Range(-spawnRange, spawnRange));
                        GameObject enemy = myPoolManager.RequestToPool(spawnerID + i, spawnPosition, new Vector3(0, Random.Range(0, 360), 0), transform.localScale);
                        if (enemy != null)
                        {
                            enemy.GetComponent<SimpleEnemyIA>().Initialize(spawnPosition, player, spawns[i].enemyLvl);
                        }                        
                    }
                    return true;
                }
            }
        }
        return false;
    }
}
