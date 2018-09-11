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
    [SerializeField] float baseMinDamage = 1;
    [SerializeField] float baseMaxDamage = 3;
    [SerializeField] float critDmg = 1.5f;
    [SerializeField] float baseCritChance = 0.05f;
    [SerializeField] int randomProperty;
    
}
