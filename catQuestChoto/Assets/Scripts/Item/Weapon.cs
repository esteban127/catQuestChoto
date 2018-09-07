using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum weaponTipe
{
    OneHandedSword,
    TwoHandedSword,
    Staf,
    Bow
}

[System.Serializable]
public class Weapon : Iitem
{

    [SerializeField] weaponTipe tipe;
    [SerializeField] float baseMinDamage;
    [SerializeField] float baseMaxDamage;
    [SerializeField] float critDmg;
    [SerializeField] float baseCritChance;
    [SerializeField] int randomProperty;

    public void SetStats(string name, int lvl, weaponTipe w_tipe, float minDmg, float maxDmg, float critMultiplicator, float critChance, int w_randomProperty, itemstats aditionalStats)
    {
        itemName = name;        
        itemLvl = lvl;
        tipe = w_tipe;
        baseMinDamage = minDmg;
        baseMaxDamage = maxDmg;
        critDmg = critMultiplicator;
        baseCritChance = critChance;
        randomProperty = w_randomProperty;
        stats = aditionalStats;
    }   
}
