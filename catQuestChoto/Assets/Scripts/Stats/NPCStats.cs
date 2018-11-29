using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : ActorStats
{

    [SerializeField] NPCActor NPC;   
    private void Start()
    {
        status = new BuffDebuffSystem();
        currentHealth = MaxHealth();
    }
    public override float MaxHealth()
    {
        return NPC.Health;
    }
    public override float MinDamage()
    {
        return NPC.minDamage;
    }
    public override float MaxDamage()
    {
        return NPC.maxDamage;
    }
    public override int Defense()
    {
        return NPC.Defense;
    }
    public override float CritDamage()
    {
        return 2;
    }
    public override float CritChance()
    {
        return 0;
    }
    public override float Precision()
    {
        return 0;
    }
    public override float DodgeChance()
    {
        return 0;
    }
    public override IACTOR getActor()
    {
        return NPC;
    }
    public override void reciveDamage(float damage)
    {
        Debug.Log("Soy un npc gil, no me deberias pegar, somos buena gente");        
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDie();//puta que sad                
        }
    }

    private void OnDie()
    {
        //Esto no deberia pasar
        gameObject.SetActive(false);        
    }        
}
