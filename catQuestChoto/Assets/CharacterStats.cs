using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    CharacterActor player;
    InventoryManager iManager;
    equipmentStats currentEquipmentStats;
    [SerializeField] StatsFrameManager frameManager;

    float currentHealth;
    float currentMana;
    int actualUnasignedPoints = 0;

    //variables de escalado
    float lifePerCons = 0.5f;
    float lifeRegenPerCons = 0.1f;
    float manaPerInt = 0.3f;
    float manaRegenPerInt = 0.1f;
    float coldownReductionPerInt = 0.002f;//porciento
    float damagePerStrength = 0.01f; //porciento
    float defensePerStrength = 0.2f;
    float dodgePerDext = 0.005f; //porciento
    float precisionPerDext = 0.005f;//porciento
    float CritPerLuck = 0.005f;//porciento
    float xpExtraPerLvl = 0.4f;
    float startingXpNeedValue = 100f;


    private void Start()
    {
        iManager = InventoryManager.Instance;
        iManager.OnStatChange += ActualizateStats;
        if (player != null)
        {
            ActualizateStats();
            ActualizateAttributePointsInFrame();
            ActualizateXpInFrame();
            actualizateLvlInFrame();
            frameManager.setName(player.Name);
        }
        //delegadeo de gameTick += Regen
    }
    public void loadCharacter(CharacterActor actor)
    {
        player = actor;        
    }    

    void Regen()
    {
        if (currentHealth < MaxHealth())
        {
            ReplenishHealt(HealtRegen());
        }
        if (currentMana < MaxMana())
        {
            ReplenishMana(ManaRegen());
        }
    }
    
    public void ReplenishHealt(float amount)
    {
        currentHealth += amount;
        if (currentHealth > MaxHealth())
            currentHealth = MaxHealth();
    }
    public void ReplenishMana(float amount)
    {
        currentMana += amount;
        if (currentMana > MaxMana())
            currentMana = MaxMana();
    }
    public void addXp(float amount)
    {
        player.Experience += amount;
        if(player.Experience>startingXpNeedValue+ (startingXpNeedValue * player.Level * xpExtraPerLvl))
        {
            float overflow = player.Experience - (startingXpNeedValue + (startingXpNeedValue * player.Level * xpExtraPerLvl));
            player.Experience = 0;
            LevelUp();
            addXp(overflow);
        }
        ActualizateXpInFrame();
    }
    public void addAttribute(attribute stat)
    {
        actualUnasignedPoints--;
        ActualizateAttributePointsInFrame();
        switch (stat)
        {
            case attribute.Strength:
                player.Strength++;
                break;
            case attribute.Constitution:
                player.Constitution++;
                break;
            case attribute.Dextery:
                player.Dextery++;
                break;
            case attribute.Inteligence:
                player.Inteligence++;
                break;
            case attribute.Luck:
                player.Luck++;
                break;
        }
        ActualizateStatsInframe();
    }

    private void LevelUp()
    {        
        player.Level++;
        actualUnasignedPoints += 5;
        ActualizateAttributePointsInFrame();
        actualizateLvlInFrame();
    }    
       
    public float MaxHealth()
    {
        return player.Health +currentEquipmentStats.stats.Health +((player.Constitution + currentEquipmentStats.stats.Constitution)*lifePerCons) ;
    }
    public float HealtRegen()
    {
        return currentEquipmentStats.stats.HealthRegen + ((player.Constitution + currentEquipmentStats.stats.Constitution) * lifeRegenPerCons);
    }
    public float MaxMana()
    {
        return player.Mana + currentEquipmentStats.stats.Mana + ((player.Inteligence + currentEquipmentStats.stats.Inteligence) * manaPerInt);
    }
    public float ManaRegen()
    {
        return currentEquipmentStats.stats.ManaRegen + ((player.Inteligence + currentEquipmentStats.stats.Inteligence) * manaRegenPerInt);
    }
    public float MinDamage()
    {
        return (player.minDamage + currentEquipmentStats.baseMinDamage)+ (player.minDamage + currentEquipmentStats.baseMinDamage) * ((player.Strength + currentEquipmentStats.stats.Strength)*damagePerStrength);
    }
    public float MaxDamage()
    {
        return (player.maxDamage + currentEquipmentStats.baseMaxDamage) + (player.maxDamage + currentEquipmentStats.baseMaxDamage) * ((player.Strength + currentEquipmentStats.stats.Strength)*damagePerStrength);
    }
    public int Defense()
    {
        return currentEquipmentStats.defense + (int)((player.Strength + currentEquipmentStats.stats.Strength) * defensePerStrength);
    }
    public float CritDamage()
    {
        return currentEquipmentStats.critDmg;
    }
    public float ColdownReduction()
    {
        return currentEquipmentStats.stats.ColdownReduction + ((player.Inteligence + currentEquipmentStats.stats.Inteligence) * coldownReductionPerInt);
    }
    public float CritChance()
    {
        return currentEquipmentStats.baseCritChance + currentEquipmentStats.stats.CritChance + ((player.Luck + currentEquipmentStats.stats.Luck) * CritPerLuck);
    }
    public float Precision()
    {
        return currentEquipmentStats.stats.Precision + ((player.Dextery + currentEquipmentStats.stats.Dextery) * precisionPerDext);
    }
    public float DodgeChance()
    {
        return currentEquipmentStats.stats.Dodge + ((player.Dextery + currentEquipmentStats.stats.Dextery) * dodgePerDext);
    }

    private void ActualizateStats()
    {
        currentEquipmentStats = iManager.CurrentEquipmentBonus;
        ActualizateStatsInframe();
    }

    private string GenerateExtraStatsText()
    {
        string extraStats = "";

        extraStats += "Damage: (" + MinDamage().ToString("F1") + " - " + MaxDamage().ToString("F1") + ") \n";
        extraStats += "Defense: " + Defense() + "\n";
        extraStats += "Max health: " + (int)MaxHealth() + "\n";
        extraStats += "Max mana: " + (int)MaxMana() + "\n";
        extraStats += "Crit damage: " + (int)(CritDamage()*100) + "%\n";
        extraStats += "Crit chance: " + (int)(CritChance()*100) + "%\n";
        extraStats += "Precision: " + (int)(Precision()*100) + "%\n";
        extraStats += "Dodge:" + (int)(DodgeChance()*100) + "%\n";
        extraStats += "Coldown reduction: " + (int)(ColdownReduction() * 100) + "%\n";
        extraStats += "Health regen: " + HealtRegen() + "/s\n";
        extraStats += "Mana regen: " + ManaRegen() + "/s\n";

        return extraStats;
    }

    //frameCalls
    private void ActualizateStatsInframe()
    {
        frameManager.SetAttribute(player.Strength, currentEquipmentStats.stats.Strength,attribute.Strength);
        frameManager.SetAttribute(player.Constitution, currentEquipmentStats.stats.Constitution, attribute.Constitution);
        frameManager.SetAttribute(player.Dextery, currentEquipmentStats.stats.Dextery, attribute.Dextery);
        frameManager.SetAttribute(player.Inteligence, currentEquipmentStats.stats.Inteligence, attribute.Inteligence);
        frameManager.SetAttribute(player.Luck, currentEquipmentStats.stats.Luck, attribute.Luck);
        frameManager.setExtraStats(GenerateExtraStatsText());
    }
    private void ActualizateAttributePointsInFrame()
    {
        frameManager.setAttributePoints(actualUnasignedPoints);
    }
    private void ActualizateXpInFrame()
    {
        frameManager.setXp((int)player.Experience, (int)(startingXpNeedValue + (startingXpNeedValue * player.Level * xpExtraPerLvl)));
    }
    private void actualizateLvlInFrame()
    {
        frameManager.setLevel(player.Level);
    }

    
}
