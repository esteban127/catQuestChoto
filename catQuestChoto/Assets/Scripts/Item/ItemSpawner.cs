using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
   
    ItemFactory Ifactory;
    InventoryManager myInventory;
    public bool spawn = true;
    QuestManager qManager;
    [SerializeField] string questNameForSpecialSpawn;
    [SerializeField] Transform[] PiedrasSpawns;
    void Start () {
        qManager = QuestManager.Instance;
        Ifactory = ItemFactory.Instance;
        myInventory = InventoryManager.Instance;
        qManager.OnNewQuestStart += SpecialQuestItemSpawn;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            if (spawn)
            {
                Ifactory.GenerateLoot(ItemTier.Tier0, transform.position); //Debug                
            }           
        }
	}

    private void SpecialQuestItemSpawn()
    {
        for (int i = 0; i < qManager.ActiveQuestKey.Count; i++)
        {
            if (qManager.ActiveQuestKey[i] == questNameForSpecialSpawn)
            {
                SpawnPiedras();
            }
        }
    }

    private void SpawnPiedras()
    {
        for (int i = 0; i < 3; i++)
        {
            Ifactory.GenerateItem(ItemTier.Tier0, ItemType.QuestItem, "Piedrita magica", PiedrasSpawns[i].position);
        }
        
    }
}
