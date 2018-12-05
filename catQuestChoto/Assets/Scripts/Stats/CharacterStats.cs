using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CharacterStats : ActorStats
{

    Clock timer;
    CharacterActor player;
    InventoryManager iManager;
    equipmentStats currentEquipmentStats;
    [SerializeField] StatsFrameManager frameManager;
    [SerializeField] SkillTreeManager skillTree;
    [SerializeField] GameObject deathPanel;
    float currentMana;
    public float CurrentMana { get { return currentMana; } }    
    SaveLoad sLManager;
   

    //variables de escalado
    float lifePerCons = 1f;
    float lifeRegenPerCons = 0.05f;
    float manaPerInt = 1.0f;
    float manaRegenPerInt = 0.075f;
    float coldownReductionPerInt = 0.002f;//porciento
    float damagePerStrength = 0.01f; //porciento
    float defensePerStrength = 0.2f;
    float dodgePerDext = 0.005f; //porciento
    float precisionPerDext = 0.005f;//porciento
    float CritPerLuck = 0.005f;//porciento
    float xpExtraPerLvl = 0.15f;
    float startingXpNeedValue = 300f;

    
    private void MiddleLoad()
    {
        Load();
        status = new BuffDebuffSystem();
        currentMana = MaxMana();
        currentHealth = MaxHealth();
    }
    private void EndOfLoad()
    {
        if (player != null)
        {
            status.onStatusChange += ActualizateStats;
            iManager.OnStatChange += ActualizateStats;
            iManager.OnConsume += getConsumable;
            ActualizateStats();
            ActualizateAttributePointsInFrame();
            ActualizateXpInFrame();
            actualizateLvlInFrame();
            frameManager.setName(player.Name);
            timer.OnTick += ActualizateLife;
        }
    }
    private void Start()
    {
        sLManager = SaveLoad.Instance;
        SaveLoad.BeforeClosing += Save;
        timer = Clock.Instance;
        iManager = InventoryManager.Instance;
        LoadSystem.OnEndLoading += EndOfLoad;
        LoadSystem.OnMidleLoading += MiddleLoad;
    }
    private void OnDisable()
    {
        LoadSystem.OnEndLoading -= EndOfLoad;
        LoadSystem.OnMidleLoading -= MiddleLoad;
    }
    private void Load()
    {
        string path = sLManager.SaveDirectory + "/Stats.json";
        CharacterActor stats = JsonUtility.FromJson<CharacterActor>(File.ReadAllText(path));
        UnityEngine.Object model = Resources.Load("CharacterPrefab/" + stats.Class);
        Instantiate(model, transform);
        player =stats;
    }

    public void Save()
    {
        Debug.Log("Save");
        string path = sLManager.SaveDirectory + "/Stats.json";        
        string save = JsonUtility.ToJson(player);

        File.WriteAllText(path, save);
    }

    private void getConsumable(Consumables itemToUse)
    {
        for (int i = 0; i < itemToUse.Buff.Length; i++)
        {
            status.addBuff(itemToUse.Buff[i]);
        }
        for (int i = 0; i < itemToUse.Debuff.Length; i++)
        {
            status.addDebuff(itemToUse.Debuff[i]);
        }
        if(itemToUse.Name == "Townportal scroll")
        {
            sLManager.ChangeScene("Town");
        }
    }

    void ActualizateLife(float time)
    {
        if (alive)
        {
            if (currentHealth < MaxHealth())
            {
                ReplenishHealt(HealtRegen() * time);
            }
            else
            {
                currentHealth = MaxHealth();
            }
            if (currentMana < MaxMana())
            {
                ReplenishMana(ManaRegen() * time);
            }
            else
            {
                currentMana = MaxMana();
            }
            reciveDamage(status.getDebuffPotency(DebuffType.damageOverTime) * 50 * time);
        }        
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
        player.UnasignedAtributePoints--;
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
        ReplenishHealt(9999);        
        skillTree.AddSkillPoint();
        player.UnasignedAtributePoints += 5;
        ActualizateAttributePointsInFrame();
        actualizateLvlInFrame();
    }    
       
    public override float MaxHealth()
    {
        return player.Health +currentEquipmentStats.stats.Health +((player.Constitution + currentEquipmentStats.stats.Constitution)*lifePerCons) ;
    }
    public float HealtRegen()
    {
        return currentEquipmentStats.stats.HealthRegen + ((player.Constitution + currentEquipmentStats.stats.Constitution) * lifeRegenPerCons) + status.getBuffPotency(BuffType.hpRegen)*50;
    }
    public float MaxMana()
    {
        return player.Mana + currentEquipmentStats.stats.Mana + ((player.Inteligence + currentEquipmentStats.stats.Inteligence) * manaPerInt);
    }
    public float ManaRegen()
    {
        return currentEquipmentStats.stats.ManaRegen + ((player.Inteligence + currentEquipmentStats.stats.Inteligence) * manaRegenPerInt) + status.getBuffPotency(BuffType.manaRegen)*50;
    }
    public override float MinDamage()
    {
        float minDamage = (player.minDamage + currentEquipmentStats.baseMinDamage) + (player.minDamage + currentEquipmentStats.baseMinDamage) * ((player.Strength + currentEquipmentStats.stats.Strength) * damagePerStrength);
        if (status.getBuffPotency(BuffType.damageBuff) > 0)
        {
            minDamage += (minDamage * status.getBuffPotency(BuffType.damageBuff));
        }
        if (status.getDebuffPotency(DebuffType.damageReduction) < 0)
        {
            minDamage -= (minDamage * status.getDebuffPotency(DebuffType.damageReduction));
        }
        return minDamage;
    }
    public override float MaxDamage()
    {
        float maxDamage = (player.maxDamage + currentEquipmentStats.baseMaxDamage) + (player.maxDamage + currentEquipmentStats.baseMaxDamage) * ((player.Strength + currentEquipmentStats.stats.Strength) * damagePerStrength);
        if (status.getBuffPotency(BuffType.damageBuff) > 0)
        {
            maxDamage += (maxDamage * status.getBuffPotency(BuffType.damageBuff));
        }
        if (status.getDebuffPotency(DebuffType.damageReduction) < 0)
        {
            maxDamage -= (maxDamage * status.getDebuffPotency(DebuffType.damageReduction));
        }
        return maxDamage;
    }
    public override int Defense()
    {
        int def = currentEquipmentStats.defense + (int)((player.Strength + currentEquipmentStats.stats.Strength) * defensePerStrength)+ (int)status.getBuffPotency(BuffType.defenseBuff) - (int)status.getDebuffPotency(DebuffType.defenseReduction);
        return def + (int)(def * status.getBuffPotency(BuffType.defenseBuff) - def * status.getDebuffPotency(DebuffType.defenseReduction));
    }
    public override float CritDamage()
    {
        return currentEquipmentStats.critDmg;
    }
    public float ColdownReduction()
    {
        return currentEquipmentStats.stats.ColdownReduction + ((player.Inteligence + currentEquipmentStats.stats.Inteligence) * coldownReductionPerInt);
    }
    public override float CritChance()
    {
        return currentEquipmentStats.baseCritChance + currentEquipmentStats.stats.CritChance + ((player.Luck + currentEquipmentStats.stats.Luck) * CritPerLuck) + status.getBuffPotency(BuffType.critChanceBuff);
    }
    public override float Precision()
    {
        return currentEquipmentStats.stats.Precision + ((player.Dextery + currentEquipmentStats.stats.Dextery) * precisionPerDext) + status.getBuffPotency(BuffType.precisionBuff);
    }
    public override float DodgeChance()
    {
        return currentEquipmentStats.stats.Dodge + ((player.Dextery + currentEquipmentStats.stats.Dextery) * dodgePerDext) + status.getBuffPotency(BuffType.dodgeBuff);
    }

    public bool TryToSpendMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }
        return false;
    }

    public override void reciveDamage(float damage)
    {
        if (alive)
        {
            if (damage > 0)
            {
                if (status.getBuffPotency(BuffType.shield) > 0)
                    status.ReduceBuffPotency((damage - (damage * (Defense() / (Defense() + 100)))) / 100, BuffType.shield);
                else
                    currentHealth -= (damage - (damage * (Defense() / (Defense() + 100))));
                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    OnDie();              
                }
            }
        }              
    }       

    public override IACTOR getActor()
    {
        return player;
    }

    private void OnDie()
    {
        alive = false;
        status.RemoveAllBuffAndDebuff();
        XpPenalty();
        deathPanel.SetActive(true);
    }

    private void XpPenalty()
    {
        player.Experience = player.Experience * 0.5f;
        ActualizateXpInFrame();
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
        frameManager.setAttributePoints(player.UnasignedAtributePoints);
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
