using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum fileTipe
{
   character,
   item
}

[System.Serializable]
public class JsonCreator : MonoBehaviour {

    Weapon weapon;
    
    Consumables consumable;
    Armor armor;
    CharacterActor player;
    
    [SerializeField] fileTipe fileTipe;
 
    //general
    [SerializeField] int ID;
    [SerializeField] string Name;
    [SerializeField] int Lvl;

    //character specific
    [SerializeField] c_class c_class;
    [SerializeField] int baseStrength;
    [SerializeField] int baseConstitution;
    [SerializeField] int baseDextery;
    [SerializeField] int baseInteligence;
    [SerializeField] int baseLuck;

    //item specific
    [SerializeField] itemTipe itemTipe;
    [SerializeField] int[,] size;
    [SerializeField] itemstats generalStats;
    [SerializeField] int randomProperty;

    //consumible specific
    [SerializeField] bool isConsumible;
    [SerializeField] float duration;

    //armor specific
    [SerializeField] armorTipe a_tipe;
    [SerializeField] int defense;

    //weapon specific
    [SerializeField] weaponTipe w_tipe;    
    [SerializeField] float minDmg;
    [SerializeField] float maxDmg;
    [SerializeField] float critDmg;
    [SerializeField] float baseCritChance;

    
    private void Awake()
    {
        
        weapon = new Weapon();
        consumable = new Consumables();
        armor = new Armor();
        player = new CharacterActor();
        
    }
    private void Start()
    {
        weapon.SetStats("Sword01", 1, weaponTipe.OneHandedSword, 3.0f, 5.0f, 1.5f, 0.15f,2, generalStats);
        consumable.setStats("Health potion", 1, true, 5.0f, generalStats);
        armor.SetStats("Shield",1,armorTipe.offHand,8.0f,2, generalStats);
        string myWeapon = JsonUtility.ToJson(player);
        Debug.Log(myWeapon);
    }

    public void GenerateFile()
    {

    }
}
