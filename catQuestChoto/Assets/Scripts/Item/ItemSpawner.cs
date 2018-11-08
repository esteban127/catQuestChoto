using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
   
    ItemFactory Ifactory;
    InventoryManager myInventory;
    public bool spawn = true;
  

    void Start () {
        Ifactory = ItemFactory.Instance();
        myInventory = InventoryManager.Instance;
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

    
}
