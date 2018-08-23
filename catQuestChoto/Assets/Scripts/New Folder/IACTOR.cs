using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum baseAttribute
{
    Health,
    Damage,
    Mana,
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

abstract class IACTOR : MonoBehaviour { 


    //basics
    protected string Id;
    protected float level;
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
    protected float baseStrength;
    protected float baseConstitution;
    protected float baseDextery;
    protected float baseInteligence;
    protected float baseLuck;
    //Equipament
    //protected Equipament equipament 
    //variable attributes
    protected float currentHealth;
    protected float currentMana;
    //Buffs
        //protected Buff activeBuffs;    

    //Functions

    //Basics
    public abstract string getName();
    public abstract float getLevel();
    public abstract Sprite getPict();
    //Health related
    protected abstract float getMaxHealth();
    public abstract float getCurrentHealth();
    public abstract float takeDamage();
    protected abstract float replenishHealth();
    //Mana related
    protected abstract float getMaxMana();
    public abstract float getCurrentMana();
    protected abstract float spendMana();
    protected abstract float replenishMana();
    //Damage related
    public abstract float getDamage();
    protected abstract float calculateFinalDamage(IACTOR target);
    //Defense related
    public abstract float getDefense();
    //Chance related
    public abstract float getDodge();
    public abstract float getPrecision();
    public abstract float getCritChance();
    protected abstract bool calculateHitSuccess(IACTOR target);
    //attribute related
    protected abstract float getAttribute(attribute attribute);

    
}
