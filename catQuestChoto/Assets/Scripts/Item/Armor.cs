using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum armorType //pendiente
{
    offHand,
    helmet,
    chest,
    pants,
    ring,
    amulet,
    boots,
    gloves,
    shield
}
[System.Serializable]
public class Armor : Iitem {


    [SerializeField] armorType type;
    public armorType ArmorType { get { return type; } }
    [SerializeField] float Defense;
    [SerializeField] int randomProperty;
    
}
