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
    float currentMana;
    public float CurrentMana { get { return currentMana; } }
    int actualUnasignedAtributePoints = 0;
    SaveLoad sLManager;
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

    private void Awake()
    {
        sLManager = SaveLoad.Instance;
        Load();
    }

    private void Start()
    {        
        timer = Clock.Instance;
        iManager = InventoryManager.Instance;
        if (player != null)
        {
            status = new BuffDebuffSystem();
            iManager.OnStatChange += ActualizateStats;
            iManager.OnConsume += getConsumable;
            ActualizateStats();
            ActualizateAttributePointsInFrame();
            ActualizateXpInFrame();
            actualizateLvlInFrame();
            frameManager.setName(player.Name);
            currentHealth = MaxHealth();
            currentMana = MaxMana();
            timer.OnTick += ActualizateLife;
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Save();
        }
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
    }

    void ActualizateLife(float time)
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
            ReplenishMana(ManaRegen()*time);
        }
        else
        {
            currentMana = MaxMana();
        }
        reciveDamage(status.getDebuffPotency(DebuffType.damageOverTime) * time);
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
        actualUnasignedAtributePoints--;
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
        actualUnasignedAtributePoints += 5;
        ActualizateAttributePointsInFrame();
        actualizateLvlInFrame();
    }    
       
    public override float MaxHealth()
    {
        return player.Health +currentEquipmentStats.stats.Health +((player.Constitution + currentEquipmentStats.stats.Constitution)*lifePerCons) ;
    }
    public float HealtRegen()
    {
        return currentEquipmentStats.stats.HealthRegen + ((player.Constitution + currentEquipmentStats.stats.Constitution) * lifeRegenPerCons) + status.getBuffPotency(BuffType.hpRegen);
    }
    public float MaxMana()
    {
        return player.Mana + currentEquipmentStats.stats.Mana + ((player.Inteligence + currentEquipmentStats.stats.Inteligence) * manaPerInt);
    }
    public float ManaRegen()
    {
        return currentEquipmentStats.stats.ManaRegen + ((player.Inteligence + currentEquipmentStats.stats.Inteligence) * manaRegenPerInt) + status.getBuffPotency(BuffType.manaRegen);
    }
    public override float MinDamage()
    {
        float minDamage = (player.minDamage + currentEquipmentStats.baseMinDamage) + (player.minDamage + currentEquipmentStats.baseMinDamage) * ((player.Strength + currentEquipmentStats.stats.Strength) * damagePerStrength);
        if (status.getBuffPotency(BuffType.damageMultpy) > 0)
        {
            minDamage += (minDamage * status.getBuffPotency(BuffType.damageMultpy));
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
        if (status.getBuffPotency(BuffType.damageMultpy) > 0)
        {
            maxDamage += (maxDamage * status.getBuffPotency(BuffType.damageMultpy));
        }
        if (status.getDebuffPotency(DebuffType.damageReduction) < 0)
        {
            maxDamage -= (maxDamage * status.getDebuffPotency(DebuffType.damageReduction));
        }
        return maxDamage;
    }
    public override int Defense()
    {
        return currentEquipmentStats.defense + (int)((player.Strength + currentEquipmentStats.stats.Strength) * defensePerStrength)+ (int)status.getBuffPotency(BuffType.defenseBuff);
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
        if(damage> 0)
        {
            if (status.getBuffPotency(BuffType.shield) > 0)
                status.ReduceBuffPotency((damage - (damage * (Defense() / (Defense() + 100)))), BuffType.shield);
            else
                currentHealth -= (damage - (damage * (Defense() / (Defense() + 100))));
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDie();//puta que sad                
            }
        }        
    }       

    public override IACTOR getActor()
    {
        return player;
    }

    private void OnDie()
    {
        Debug.Log("te moriste we"); 
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
        frameManager.setAttributePoints(actualUnasignedAtributePoints);
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
