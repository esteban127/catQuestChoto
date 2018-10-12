using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct itemstats
{
    [SerializeField] float Health;
    [SerializeField] float Mana;
    [SerializeField] float Precision;
    [SerializeField] float Dodge;
    [SerializeField] float CritChance;
    [SerializeField] float Strength;
    [SerializeField] float Constitution;
    [SerializeField] float Dextery;
    [SerializeField] float Inteligence;
    [SerializeField] float Luck;
    [SerializeField] float coldownReduction;
    [SerializeField] float heathRegen;
    [SerializeField] float manaRegen;
}

public enum ItemImage
{
    Sword0,
    PlaceHolder,    
}

[System.Serializable]
public abstract class Iitem {

    [SerializeField] int[] size;
    public int[] Size { get { return size; } }
    [SerializeField] protected int itemLvl;
    [SerializeField] protected itemstats stats;
    [SerializeField] protected string ItemID = "I1example";
    public string ID { get { return ItemID; } }
    [SerializeField] protected string itemName;
    public string Name { get { return itemName; } }
    [SerializeField] protected ItemImage image;
    public ItemImage Image { get { return image; } }
    
    //public abstract void Use();
    void loadStats()
    {

    }

    public Sprite getImage()
    {

        Sprite itemImage = null;
        switch (image)
        {
            case ItemImage.Sword0:
                itemImage = Resources.Load<Sprite>("Art/ItemSprite/Sword0");
                break;

        }
        return itemImage;
    }
}
