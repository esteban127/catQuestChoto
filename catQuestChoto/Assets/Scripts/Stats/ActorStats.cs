using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorStats : MonoBehaviour {

    public BuffDebuffSystem status;    
    protected float currentHealth;
    public float CurrentHealth { get { return currentHealth; } }
    protected bool alive = true;
    public bool Alive { get { return alive; } }
    public abstract IACTOR getActor();
    public abstract void  reciveDamage(float damage);
    public abstract float MaxHealth();
    public abstract float MinDamage();
    public abstract float MaxDamage();
    public abstract int Defense();
    public abstract float CritDamage();
    public abstract float CritChance();
    public abstract float Precision();
    public abstract float DodgeChance();

    public void ReplenishHealt(float amount)
    {
        currentHealth += amount;
        if (currentHealth > MaxHealth())
            currentHealth = MaxHealth();
        if (currentHealth <= 0)
        {
            currentHealth = 1;
        }
    }
    public void reciveBuff(BuffDebuffSystem.Buff buff)
    {
        status.addBuff(buff);
    }
    public void reciveDebuff(BuffDebuffSystem.Debuff debuff)
    {        
        status.addDebuff(debuff);
    }
}
