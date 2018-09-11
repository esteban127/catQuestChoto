using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum baseAttribute
{
    Health,
    Damage,
    Mana,
    Defense,
    Precision,
    Dodge,
    CritChance
}

enum  attribute
{
    Strength,
    Constitution,
    Dextery,
    Inteligence,
    Luck
}

public abstract class IACTOR {


    //basics
    [SerializeField] protected int actorID;
    public int ID { get { return actorID; } }
    protected int level;
    protected Sprite profilePict;
    //base attributes    
    protected float baseHealth;
    protected float baseDamage;
    protected float baseDefense;
    protected float baseMana;
    protected float baseDodge;
    protected float basePrecision;
    protected float baseCritChance;
    //attributes
    [SerializeField] protected int baseStrength = 5;
    [SerializeField] protected int baseConstitution = 5;
    [SerializeField] protected int baseDextery = 5;
    [SerializeField] protected int baseInteligence = 5;
    [SerializeField] protected int baseLuck = 5;
    //Equipament
    //protected Equipament equipament 
    //variable attributes
     protected float currentHealth;
     protected float currentMana;
    //Buffs
    //protected Buff activeBuffs;    

    //Functions
    /*
    //Basics
    public string getName() { };
    public float getLevel() { };
    public Sprite getPict() { };
    //Health related
    protected float getMaxHealth() { };
    public float getCurrentHealth() { };
    public float takeDamage() { };
    protected float replenishHealth() { };
    //Mana related
    protected float getMaxMana() { };
    public float getCurrentMana() { };
    protected float spendMana() { };
    protected float replenishMana();
    //Damage related
    public float getDamage() { };
    protected float calculateFinalDamage(IACTOR target) { };
    //Defense related
    public float getDefense() { };
    //Chance related
    public float getDodge() { };
    public float getPrecision() { };
    public float getCritChance() { };
    protected bool calculateHitSuccess(IACTOR target) { };
    //attribute related
    protected float getAttribute(attribute attribute) { };
    */
    
}
