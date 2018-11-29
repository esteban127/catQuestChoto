using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum weaponType
{
    OneHanded,
    TwoHanded,
    MagicStaf,
    Bow,
}

[System.Serializable]
public class Weapon : Iitem
{

    [SerializeField] weaponType type;  
    [SerializeField] float baseMinDamage = 1;   
    [SerializeField] float baseMaxDamage = 3;
    [SerializeField] float critDmg = 1.5f;
    [SerializeField] float baseCritChance = 0.05f;
    [SerializeField] int randomProperty;
    [SerializeField] itemstats aditionalStats;

    public weaponType WeaponType { get { return type; } }
    public float BaseMinDamage { get { return baseMinDamage; } }
    public float BaseMaxDamage { get { return baseMaxDamage; } }
    public float CritDmg { get { return critDmg; } }
    public float BaseCritChance { get { return baseCritChance; } }
    public itemstats AditionalStats { get { return aditionalStats; } }

    public void SetRandomProperty()
    {
        List<int> alreadyRolled = new List<int>();
        int roll;
        int mainStat;
        float percStat;
        float regenStat;
        for (int i = 0; i < randomProperty; i++)
        {
            do
            {
                roll = Random.Range(0, 12);
            } while (alreadyRolled.Contains(roll));
            alreadyRolled.Add(roll);

            mainStat = (Random.Range(1, 10) + ((int)tier * 5));
            percStat = ((float)Random.Range(1, 5) * 0.01f) + (float)tier * 0.025f;
            regenStat = (float)Random.Range(1, 5)*0.5f  + (float)tier * 1.5f + 1f;
            switch (type)
            {
                case weaponType.TwoHanded:
                    mainStat *= 4;
                    break;
                case weaponType.Bow:
                    percStat *= 4;
                    break;
                case weaponType.MagicStaf:
                    regenStat *= 4;
                    break;
            }
            switch (roll)
            {
                case 0:
                    aditionalStats.Health = mainStat;
                    break;
                case 1:
                    aditionalStats.Mana = mainStat;
                    break;
                case 2:
                    aditionalStats.Strength = mainStat;
                    break;
                case 3:
                    aditionalStats.Constitution = mainStat;
                    break;
                case 4:
                    aditionalStats.Dextery = mainStat;
                    break;
                case 5:
                    aditionalStats.Inteligence = mainStat;
                    break;
                case 6:
                    aditionalStats.Luck = mainStat;
                    break;
                case 7:
                    aditionalStats.Precision = percStat;
                    break;
                case 8:
                    aditionalStats.Dodge = percStat;
                    break;
                case 9:
                    aditionalStats.CritChance = percStat;
                    break;
                case 10:
                    aditionalStats.ColdownReduction = percStat;
                    break;
                case 11:
                    aditionalStats.HealthRegen = regenStat;
                    break;
                case 12:
                    aditionalStats.ManaRegen = regenStat;
                    break;
            }


        }
    }

}
