using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum armorType //pendiente
{
    offHand,
    helmet,
    chest,
    pants,
    ring,
    amulet,
    boots,
    gloves,    
}
[System.Serializable]
public class Armor : Iitem {


    [SerializeField] armorType type;
    [SerializeField] int defense;
    [SerializeField] int randomProperty;
    [SerializeField] itemstats aditionalStats;


    public armorType ArmorType { get { return type; } }
    public itemstats AditionalStats { get { return aditionalStats; } }
    public int Defense { get { return defense; } }


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
            regenStat = (float)Random.Range(1, 5) * 0.5f + (float)tier * 1.5f +1f;
            switch (type)
            {                
                case armorType.chest:
                case armorType.pants:
                    mainStat *= 2;
                    break;
                case armorType.ring:
                case armorType.amulet:
                    percStat *= 2.0f;
                    break;
                case armorType.boots:
                case armorType.offHand:
                case armorType.helmet:
                case armorType.gloves:
                    regenStat *= 2.0f;
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
