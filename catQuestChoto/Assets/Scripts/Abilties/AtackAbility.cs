
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AtackAbility : IAbility{

    [SerializeField] float abilityDamageMultiplier = 1;
    [SerializeField] float damagePerLevel = 0.1f;
    public float DamageMultiplier { get { return abilityDamageMultiplier + damagePerLevel*level; } }

    public override bool TryCastAbility(GameObject target, GameObject caster, string validTargetTag)
    {
        if (!loked)
        {
            if (!onCooldown)
            {
                if (target != null && target.tag == validTargetTag)
                {
                    if ((target.transform.position - caster.transform.position).magnitude < range)
                    {
                        return CastAbility(target.GetComponent<ActorStats>(), caster.GetComponent<ActorStats>());
                         
                    }
                }
            }
        }               
        return false;
    }

    private bool CastAbility(ActorStats target, ActorStats caster)
    {
        if(caster.GetType() == typeof(CharacterStats))
        {
            if (((CharacterStats)caster).TryToSpendMana(manaCost))
            {
                ResetCooldown();
                return true;
            }
            return false;
        }
        else
        {
            ResetCooldown();
            return true;
        }
    }
    public override void CastEffect(ActorStats target, ActorStats caster)
    {
        if (calculateIfHits(caster, target))
        {
            ApplyBuffAndDebuff(target, caster);
            target.reciveDamage((int)(calculateDamage(caster) * (abilityDamageMultiplier + damagePerLevel * level)));
        }
    }
    private void ApplyBuffAndDebuff(ActorStats target, ActorStats caster)
    {
        BuffDebuffSystem.Debuff debuff;
        BuffDebuffSystem.Buff buff;
        for (int i = 0; i < debuffToAply.Length; i++)
        {
            debuff = new BuffDebuffSystem.Debuff(debuffToAply[i].type, debuffToAply[i].potency + (debuffPotencyPerLevel[i] * level), debuffToAply[i].remainTime);
            target.reciveDebuff(debuff);
        }
        for (int i = 0; i < buffToAply.Length; i++)
        {
            buff = new BuffDebuffSystem.Buff(buffToAply[i].type, buffToAply[i].potency + (buffPotencyPerLevel[i]*level), buffToAply[i].remainTime);
            caster.reciveBuff(buff);
        }
    }

    private float calculateDamage(ActorStats caster)
    {        
        float damage = Random.Range(caster.MinDamage(), caster.MaxDamage());
        float roll = Random.Range(0.0f, 1.0f);
        float chanceTocrit = caster.CritChance();
        if (roll < chanceTocrit)
        {
            damage *= caster.CritDamage();
        }

        return damage;

    }

    private bool calculateIfHits(ActorStats caster, ActorStats target)
    {
        float roll = Random.Range(0.0f, 1.0f);
        float chanceToDodge = target.DodgeChance() - caster.Precision();        
        if (roll < chanceToDodge)
        {            
            return false;
        }
        else
            return true;
    }
    
}
