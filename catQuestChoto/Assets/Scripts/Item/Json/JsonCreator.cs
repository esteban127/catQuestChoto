using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonCreator : MonoBehaviour {

    Weapon weapon;
    itemstats weaponStats;
    Consumables consumable;

    private void Awake()
    {
        
        weapon = new Weapon();
        consumable = new Consumables(); 
        
    }
    private void Start()
    {
        weapon.SetStats("Sword01", "it's a sword", 1, weaponTipe.OneHandedSword, 3.0f, 5.0f, 1.5f, 0.15f, weaponStats);
        consumable.setStats("Health potion", "Restore some health", 1,true, 5, weaponStats);
        string myWeapon = JsonUtility.ToJson(consumable);
        Debug.Log(myWeapon);
    }
}
