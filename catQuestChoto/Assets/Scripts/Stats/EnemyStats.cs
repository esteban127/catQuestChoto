using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : ActorStats
{

    [SerializeField] EnemyActor enemy;
    [SerializeField] string questDrop; 
    ItemFactory iFactory;    
    QuestManager qManager;    
    Clock timer;
   

    public void Initialize(int level)
    {
        alive = true;
        timer = Clock.Instance;
        qManager = QuestManager.Instance;
        status = new BuffDebuffSystem();
        enemy.Level = level;        
        iFactory = ItemFactory.Instance;
        currentHealth = MaxHealth();
        timer.OnTick += ActualizateLife;
    }    

    //variables de escalado
    float lifePerLevel = 0.3f;
    float precisionPerLevel= 0.1f;
    float dodgePerLevel = 0.1f;
    float damagePerLevel = 0.1f;
    float defensePerLevel= 0.1f;
    float xpPerLevel = 0.1f;


    void ActualizateLife(float time)
    {
        if (alive)
        {
            if (currentHealth < MaxHealth())
            {
                ReplenishHealt(status.getBuffPotency(BuffType.hpRegen) * 50 * time);
            }
            else
            {
                currentHealth = MaxHealth();
            }
            reciveDamage(status.getDebuffPotency(DebuffType.damageOverTime) * 50 * time);
        }        
    }   

    public override float MaxHealth()
    {
        return enemy.Health + enemy.Health * lifePerLevel* enemy.Level;
    }        
    public override float MinDamage()
    {
        float minDamage = enemy.minDamage + enemy.minDamage * damagePerLevel * enemy.Level;
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
        float maxDamage = enemy.maxDamage + enemy.minDamage * damagePerLevel * enemy.Level;
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
        int def = enemy.Defense + (int)(enemy.Defense * defensePerLevel * enemy.Level);
        return def + (int)(def * status.getBuffPotency(BuffType.defenseBuff) - def * status.getDebuffPotency(DebuffType.defenseReduction));
    }
    public override float CritDamage()
    {
        return 2;
    }   
    public override float CritChance()
    {
        return enemy.CritChance + status.getBuffPotency(BuffType.critChanceBuff);
    }
    public override float Precision()
    {
        return enemy.BasePrecision + enemy.BasePrecision * precisionPerLevel * enemy.Level + status.getBuffPotency(BuffType.precisionBuff);
    }
    public override float DodgeChance()
    {
        return enemy.BaseDodgeChance + enemy.BaseDodgeChance* dodgePerLevel * enemy.Level + status.getBuffPotency(BuffType.dodgeBuff);
    }
    public override IACTOR getActor()
    {
        return enemy;
    }
    public override void reciveDamage(float damage)
    {
        if (damage > 0)
        {            
            if (status.getBuffPotency(BuffType.shield) > 0)
                status.ReduceBuffPotency((damage - (damage * (Defense() / (Defense() + 100))))/100, BuffType.shield);
            else
                currentHealth -= (damage -(damage * (Defense() / (Defense() + 100))));
            if (currentHealth <= 0 && alive)
            {
                currentHealth = 0;
                OnDie();//puta que sad                
            }
        }       
        
    }   

    private void OnDie()
    {
        alive = false;
        status.RemoveAllBuffAndDebuff();
        qManager.OnKill(enemy);      
        GenerateDrop();
    }
    public float XpReward()
    {
        return enemy.BaseXpReward + enemy.BaseXpReward * xpPerLevel;
    }
    private void GenerateDrop()
    {        
        int chance = 0;
        int roll = Random.Range(0, 100);
        for (int i = 0; i < enemy.Drop.Length; i++)
        {
            chance += enemy.Drop[i].chance;
            if (roll < chance)
            {
                iFactory.GenerateLoot(enemy.Drop[i].tier, transform.position);
                roll = 100;
            }
        }
        if (questDrop != null && questDrop != "")
        {
            iFactory.GenerateItem(ItemTier.Tier0, ItemType.QuestItem, questDrop, transform.position);
        }
    }
}
