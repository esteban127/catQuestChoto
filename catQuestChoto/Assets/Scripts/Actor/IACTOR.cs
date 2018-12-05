using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActorImage
{
    Roberta,
    Warrior,
    Archer,
    Mage,
    Roberto,
    Pepita,
    MeleGoblin,
    RangedGoblin,
    KeyKeeper,
    Mutant,
    Zombie
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
    public string Name { get { return name; } set{ name = value;} }
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
    public int Defense { get { return baseDefense; } }
   
    


    public Sprite getImage()
    {
        Sprite itemImage = null;
        itemImage = Resources.Load<Sprite>("Art/ActorSprite/" + image);
        return itemImage;
    }

}
