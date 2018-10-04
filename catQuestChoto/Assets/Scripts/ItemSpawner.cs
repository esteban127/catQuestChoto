﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    int i = 0;
    ItemFactory Ifactory;

	// Use this for initialization
	void Start () {
        Ifactory = ItemFactory.Instance();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
           
                GenerateSwordTest();
            
        }
	}

    private void GenerateSwordTest()
    {
        GameObject Sword = Ifactory.Generate(FileType.Weapon, "0-1");
        Sword.name = Sword.GetComponent<WeaponManager>().GetName();        
        Sword.transform.position = this.transform.position;
        UnityEngine.Object Model = Resources.Load("ItemPrefab/SwordItem");
        GameObject SwordModel = (GameObject)GameObject.Instantiate(Model,Sword.transform);       
    }
}