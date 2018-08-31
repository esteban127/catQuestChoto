using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum weaponTipe
{
    OneHandedSword,
    Dager,
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

    public void SetStats(string name, string w_description, int lvl, weaponTipe w_tipe, float minDmg, float maxDmg, float critMultiplicator, float critChance, itemstats aditionalStats)
    {
        itemName = name;
        description = w_description;
        itemLvl = lvl;
        tipe = w_tipe;
        baseMinDamage = minDmg;
        baseMaxDamage = maxDmg;
        critDmg = critMultiplicator;
        baseCritChance = critChance;
        stats = aditionalStats;
    }   
}
