using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActorImage
{
    Roberta,
    Warrior,
    Archer,
    Mage
}

public enum  attribute
{
    Strength,
    Constitution,
    Dextery,
    Inteligence,
    Luck
}

public abstract class IACTOR {


    //basics
    [SerializeField] protected string name;
    public string Name { get { return name; } }
    [SerializeField] protected int level;
    public int Level { get { return level; } set { level = value; } }
    [SerializeField] protected ActorImage image;

    //base attributes    
    [SerializeField] protected int baseHealth;
    public int Health { get { return baseHealth; } }
    [SerializeField] protected float baseDamage;
    public float minDamage { get { return baseDamage*0.7f; } }
    public float maxDamage { get { return baseDamage*1.3f; } }
    [SerializeField] protected int baseDefense;
    public float Defense { get { return baseDefense; } }
    [SerializeField] protected int baseMana;
    public int Mana { get { return baseMana; } }
    //attributes
    [SerializeField] protected int baseStrength = 5;
    [SerializeField] protected int baseConstitution = 5;
    [SerializeField] protected int baseDextery = 5;
    [SerializeField] protected int baseInteligence = 5;
    [SerializeField] protected int baseLuck = 5;
    public int Strength { get { return baseStrength; } set { baseStrength = value; } }
    public int Constitution { get { return baseConstitution; } set { baseConstitution = value; } }
    public int Dextery { get { return baseDextery; } set { baseDextery = value; } }
    public int Inteligence { get { return baseInteligence; } set { baseInteligence = value; } }
    public int Luck { get { return baseLuck; } set { baseLuck = value; } }


    public Sprite getImage()
    {
        Sprite itemImage = null;
        itemImage = Resources.Load<Sprite>("Art/ActorSprite/" + image);
        return itemImage;
    }

}
