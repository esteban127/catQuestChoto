using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct itemstats
{
    public int Health;
    public int Mana;    
    public int Strength;
    public int Constitution;
    public int Dextery;
    public int Inteligence;
    public int Luck;
    public float Precision;
    public float Dodge;
    public float CritChance;
    public float ColdownReduction;
    public float HealthRegen;
    public float ManaRegen;

    public static itemstats operator +(itemstats s1, itemstats s2)
    {
        itemstats statsToreturn;
        statsToreturn.Health = s1.Health += s2.Health;
        statsToreturn.Mana = s1.Mana + s2.Mana;
        statsToreturn.Strength = s1.Strength + s2.Strength;
        statsToreturn.Constitution = s1.Constitution + s2.Constitution;
        statsToreturn.Dextery = s1.Dextery + s2.Dextery;
        statsToreturn.Inteligence = s1.Inteligence + s2.Inteligence;
        statsToreturn.Luck = s1.Luck + s2.Luck;
        statsToreturn.Precision = s1.Precision + s2.Precision;
        statsToreturn.Dodge = s1.Dodge + s2.Dodge;
        statsToreturn.CritChance = s1.CritChance + s2.CritChance;
        statsToreturn.ColdownReduction = s1.ColdownReduction + s2.ColdownReduction;
        statsToreturn.HealthRegen = s1.HealthRegen + s2.HealthRegen;
        statsToreturn.ManaRegen = s1.ManaRegen + s2.ManaRegen;
        return statsToreturn;
    }    
}

public enum ItemImage
{
    Amulet,
    Boots,
    Bow,
    Chest,
    Glove,
    Helmet,
    Pants,
    RedRing,
    BlueRing,
    Shield,
    Sword,
    HealPotion,
    Rune,
    DualHandedSword,
    Staf,
    ManaPotion,
    GreenPotion,
    YelowPotion,
    TownPortal,
    CastleKey
}
public enum ItemTier
{
    Tier0,
    Tier1,
    Tier2,
    Tier3
}

[System.Serializable]
public abstract class Iitem {

    [SerializeField] int[] size = {1,1};
    public int[] Size { get { return size; } }
    [SerializeField] protected int itemLvl;
    public int Lvl { get { return itemLvl; } }
    [SerializeField] public itemstats stats;
    [SerializeField] protected string ItemID = "I1example";
    public string ID { get { return ItemID; } }
    [SerializeField] protected string itemName;
    public string Name { get { return itemName; } }
    [SerializeField] protected ItemImage image;
    public ItemImage Image { get { return image; } }
    [SerializeField] protected ItemTier tier;
    public ItemTier Tier { get { return tier; } }
    [SerializeField] string description;
    public string Description { get { return description; } }
    //public abstract void Use();
    void loadStats()
    {

    }

    public Sprite getImage()
    {

        Sprite itemImage = null;
        itemImage = Resources.Load<Sprite>("Art/ItemSprite/"+ image + (int)tier);
        return itemImage;
    }
}
