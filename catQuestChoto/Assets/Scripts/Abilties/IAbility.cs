using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum abilitySprite
{    
    Empty,
    Fist
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
    public int CurrentCharges { get { return CurrentCharges; } }
    float cooldownReduction = 0;
    protected bool loked = false;
    public bool Loked { get { return loked; } }
    public float Cooldown { get { return baseCooldown - (baseCooldown * cooldownReduction); } }
    float remainCooldown = 0;
    public float RemainCooldown { get { return remainCooldown; } }
    protected bool onCooldown = false;
    public bool OnCooldow { get { return onCooldown; } }
    Clock timer;
    public abstract bool TryCastAbility(GameObject target, GameObject caster, string validTargetTag);


    public void SetCDR(float cdr)
    {
        cooldownReduction = cdr;
    }
    public void Initialize()
    {
        timer = Clock.Instance;
        timer.OnTick += ReduceCooldown;
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
                currentCharges = maxCharges;
                onCooldown = false;
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
