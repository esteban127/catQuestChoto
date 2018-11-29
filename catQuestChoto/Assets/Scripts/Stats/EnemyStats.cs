using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : ActorStats
{

    [SerializeField] EnemyActor enemy;
    ItemFactory iFactory;    
    QuestManager qManager;
    bool isAlive = false;
    public bool IsAlive { get { return isAlive; } }
    Clock timer;
   

    public void Initialize(int level)
    {
        isAlive = true;
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
    float precisionPerLevel= 0.2f;
    float dodgePerLevel = 0.1f;
    float damagePerLevel = 0.3f;
    float defensePerLevel= 0.2f;
    float xpPerLevel = 0.3f;


    void ActualizateLife(float time)
    {
        if (currentHealth < MaxHealth())
        {
            ReplenishHealt(status.getBuffPotency(BuffType.hpRegen) * time);
        }
        else
        {
            currentHealth = MaxHealth();
        }       
        reciveDamage(status.getDebuffPotency(DebuffType.damageOverTime) * time);
    }

    public void ReplenishHealt(float amount)
    {
        currentHealth += amount;
        if (currentHealth > MaxHealth())
            currentHealth = MaxHealth();
    }

    public override float MaxHealth()
    {
        return enemy.Health + enemy.Health * lifePerLevel* enemy.Level;
    }        
    public override float MinDamage()
    {
        float minDamage = enemy.minDamage + enemy.minDamage * damagePerLevel * enemy.Level;
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
        float maxDamage = enemy.maxDamage + enemy.minDamage * damagePerLevel * enemy.Level;
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
        return enemy.Defense + (int)(enemy.Defense * defensePerLevel * enemy.Level) + (int)status.getBuffPotency(BuffType.defenseBuff);    
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
                status.ReduceBuffPotency((damage - (damage * (Defense() / (Defense() + 100)))), BuffType.shield);
            else
                currentHealth -= (damage -(damage * (Defense() / (Defense() + 100))));
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDie();//puta que sad                
            }
        }       
        
    }   

    private void OnDie()
    {
        isAlive = false;
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
    }
}
