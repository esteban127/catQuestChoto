using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct itemstats
{
    [SerializeField] float Health;
    [SerializeField] float Damage;
    [SerializeField] float Defense;
    [SerializeField] float Mana;
    [SerializeField] float Precision;
    [SerializeField] float Dodge;
    [SerializeField] float CritChance;
    [SerializeField] float Strength;
    [SerializeField] float Constitution;
    [SerializeField] float Dextery;
    [SerializeField] float Inteligence;
    [SerializeField] float Luck;
    [SerializeField] float coldownReduction;
    [SerializeField] float heathRegen;
    [SerializeField] float manaRegen;
}
[System.Serializable]
public abstract class Iitem {

    [SerializeField] Vector2Int size;
    [SerializeField] protected int itemLvl;
    [SerializeField] protected itemstats stats;
    [SerializeField] protected string ItemID;
    [SerializeField] protected string itemName;
    [SerializeField] protected string description;  
    
    //public abstract void Use();
    void loadStats()
    {

    }
}
