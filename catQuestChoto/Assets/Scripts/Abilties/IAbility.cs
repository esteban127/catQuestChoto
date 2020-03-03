using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum abilitySprite
{    
    Empty,
    Fist,
    OneHanded,
    ShieldAlone,
    SwordAndShield,
    Bow,
    TwoHandedSword,
    Staff,
    DualBlades,
    Warrior0,
    Warrior1,
    Warrior2,
    Warrior3,
    Warrior4,
    Warrior5,
    Warrior6,
    Warrior7,
    Warrior8,
    Warrior9,
    Archer0,
    Archer1,
    Archer2,
    Archer3,
    Archer4,
    Archer5,
    Archer6,
    Archer7,
    Archer8,
    Archer9,
    Mage0,
    Mage1,
    Mage2,
    Mage3,
    Mage4,
    Mage5,
    Mage6,
    Mage7,
    Mage8,
    Mage9,
    Enemy
}
public enum abilityClass
{
    Warrior,
    Archer,
    Mage,
    Enemy,
    Weapon,
   
}

[System.Serializable]
public abstract class IAbility {

    
    [SerializeField] string abilityName;
    public string Name { get { return abilityName; } }
    [SerializeField] abilityClass abilityClass;
    public abilityClass AbilityClass { get { return abilityClass; } }
    [SerializeField] int posInTree = 0;
    public int PosInTree { get { return posInTree; } }
    [SerializeField] animTrigger abilityAniamtion;
    public animTrigger AbilityAnimation { get { return abilityAniamtion; } }
    [SerializeField] protected int level = 1;
    public int Level { get { return level; } set { level = value; } }
    [SerializeField] protected abilitySprite abilitySprite;
    [SerializeField] protected float range;
    public float Range { get { return range; } }
    [SerializeField] string description;
    public string Description { get { return description; } }
    [SerializeField] float baseCooldown;
    [SerializeField] int maxCharges = 0;
    public int MaxCrages { get { return maxCharges; } }
    [SerializeField] protected int manaCost = 0;
    public int ManaCost { get { return manaCost; } }
    [SerializeField] WeaponSet[] weaponSetReq;
    public WeaponSet[] WeaponReq { get { return weaponSetReq; } }
    int currentCharges = 0;
    public int CurrentCharges { get { return currentCharges; } }
    float cooldownReduction = 0;
    protected bool loked = false;
    bool outOfCharges = false;
    public bool Loked { get { return loked; } }
    public float Cooldown { get { return baseCooldown - (baseCooldown * cooldownReduction); } }
    float remainCooldown = 0;
    public float RemainCooldown { get { return remainCooldown; } }
    protected bool onCooldown = false;
    public bool OnCooldow { get { return onCooldown; } }
    [SerializeField] protected BuffDebuffSystem.Buff[] buffToAply;
    public BuffDebuffSystem.Buff[] Buff { get { return buffToAply; } }
    [SerializeField] protected float[] buffPotencyPerLevel;
    public float[] BuffPotencyPerLevel { get { return buffPotencyPerLevel; } }
    [SerializeField] protected BuffDebuffSystem.Debuff[] debuffToAply;
    public BuffDebuffSystem.Debuff[] Debuff { get { return debuffToAply; } }
    [SerializeField] protected float[] debuffPotencyPerLevel;
    public float[] DebuffPotencyPerLevel { get { return debuffPotencyPerLevel; } }
    Clock timer;
    public abstract bool TryCastAbility(GameObject target, GameObject caster, string validTargetTag);
    public abstract void CastEffect(ActorStats target, ActorStats caster);




    public void SetCDR(float cdr)
    {
        cooldownReduction = cdr;
    }
    public void Initialize()
    {
        timer = Clock.Instance;
        timer.OnTick += ReduceCooldown;
        currentCharges = maxCharges;
    }
    public void Lock()
    {
        loked = true;
    }
    public void Unlock()
    {
        loked = false;
    }

    private void ReduceCooldown(float time)
    {
        if (onCooldown)
        {
            remainCooldown -= time;
            if (remainCooldown <= 0)
            {                
                onCooldown = false;
                if (outOfCharges)
                {
                    currentCharges = maxCharges;
                    outOfCharges = false;
                }
            }
        }
    }
    protected void ResetCooldown()
    {
        if (currentCharges > 1)
        {
            currentCharges--;
        }
        else
        {
            outOfCharges = true;
            onCooldown = true;
            remainCooldown = Cooldown;
        }
    }
    public void PutInGlobarColdown()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            remainCooldown = 0.6f;
        }
    }
     
    public Sprite AbilitySprite
    {
        get
        {
            Sprite itemImage = null;
            itemImage = Resources.Load<Sprite>("Art/AbilitySprite/" + abilitySprite);
            return itemImage;
        }
    }
}
