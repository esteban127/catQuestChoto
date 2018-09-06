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

public abstract class IACTOR : MonoBehaviour {


    //basics
    [SerializeField] protected string Id;
    [SerializeField] protected int level;
    protected Sprite profilePict;
    //base attributes    
    [SerializeField] protected float baseHealth;
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float baseDefense;
    [SerializeField] protected float baseMana;
    [SerializeField] protected float baseDodge;
    [SerializeField] protected float basePrecision;
    [SerializeField] protected float baseCritChance;
    //attributes
    [SerializeField] protected int baseStrength;
    [SerializeField] protected int baseConstitution;
    [SerializeField] protected int baseDextery;
    [SerializeField] protected int baseInteligence;
    [SerializeField] protected int baseLuck;
    //Equipament
    //protected Equipament equipament 
    //variable attributes
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float currentMana;
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
