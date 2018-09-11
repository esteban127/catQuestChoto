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
[System.Serializable]
public class Armor : Iitem {


    [SerializeField] armorTipe tipe;
    [SerializeField] float Defense;
    [SerializeField] int randomProperty;
    
}
