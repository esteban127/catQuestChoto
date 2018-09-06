using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum armorTipe //pendiente
{
    offHand,
    helmet,
    chest,
    pants,
    ring,
    amulet,
    boots,
    gloves
}

public class Armor : Iitem {


    [SerializeField] armorTipe tipe;
    [SerializeField] float Defense;
    [SerializeField] int randomProperty;

    public void SetStats(string name, int lvl, armorTipe a_tipe, float a_Defense, int w_randomProperty, itemstats aditionalStats)
    {
        itemName = name;
        itemLvl = lvl;
        tipe = a_tipe;
        Defense = a_Defense;        
        randomProperty = w_randomProperty;
        stats = aditionalStats;
    }

}
