using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct itemstats
{
    int Lvl;
    float Health;
    float Damage;
    float Defense;
    float Mana;
    float Precision;
    float Dodge;
    float CritChance;
    float Strength;
    float Constitution;
    float Dextery;
    float Inteligence;
    float Luck;
    float coldownReduction;
    float heathRegen;
    float manaRegen;
}

public abstract class Iitem : MonoBehaviour {

    int size;
    itemstats stats;
    GameObject item;
    string itemName;
    string description;    
    public abstract void Use();
    void loadStats()
    {

    }
}
