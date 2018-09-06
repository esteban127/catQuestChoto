using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonCreator : MonoBehaviour {

    Weapon weapon;
    itemstats weaponStats;
    Consumables consumable;
    Armor armor;
    CharacterActor player;
    private void Awake()
    {
        
        weapon = new Weapon();
        consumable = new Consumables();
        armor = new Armor();
        player = new CharacterActor();
        
    }
    private void Start()
    {
        weapon.SetStats("Sword01", 1, weaponTipe.OneHandedSword, 3.0f, 5.0f, 1.5f, 0.15f,2, weaponStats);
        consumable.setStats("Health potion", 1, true, 5.0f, weaponStats);
        armor.SetStats("Shield",1,armorTipe.offHand,8.0f,2,weaponStats);
        string myWeapon = JsonUtility.ToJson(player);
        Debug.Log(myWeapon);
    }
}
