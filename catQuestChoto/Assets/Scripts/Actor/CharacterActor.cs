using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum c_class
{
    Warrior,
    Ranger,
    Mage
}


[System.Serializable]
public class CharacterActor : FriendlyActor {

    [SerializeField] protected int baseMana;
    public int Mana { get { return baseMana; } }
    [SerializeField] c_class characterClass;
    public c_class Class { get { return characterClass; } }
    [SerializeField] float experience;
    public float Experience { get { return experience; } set { experience = value; } }    
    //attributes
    [SerializeField] protected int baseStrength = 5;
    [SerializeField] protected int baseConstitution = 5;
    [SerializeField] protected int baseDextery = 5;
    [SerializeField] protected int baseInteligence = 5;
    [SerializeField] protected int baseLuck = 5;
    [SerializeField] int unasignedAtributePoints = 0;
    public int Strength { get { return baseStrength; } set { baseStrength = value; } }
    public int Constitution { get { return baseConstitution; } set { baseConstitution = value; } }
    public int Dextery { get { return baseDextery; } set { baseDextery = value; } }
    public int Inteligence { get { return baseInteligence; } set { baseInteligence = value; } }
    public int Luck { get { return baseLuck; } set { baseLuck = value; } }
    public int UnasignedAtributePoints { get { return unasignedAtributePoints; }set { unasignedAtributePoints = value; } }
    //[SerializeField] skilltree skilltree?

}
