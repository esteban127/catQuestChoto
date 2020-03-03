using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealAbility : IAbility {

    [SerializeField] float healAmount = 1;
    [SerializeField] float healPerLevel = 0.1f;
    public float Heal { get { return healAmount + healPerLevel * level; } }


    public override bool TryCastAbility(GameObject target, GameObject caster, string validTargetTag)
    {
        if (!loked)
        {
            if (!onCooldown)
            {

                return CastAbility(caster.GetComponent<ActorStats>());
                 
                
                
            }
        }
        return false;
    }

    private bool CastAbility(ActorStats caster)
    {
        if (caster.GetType() == typeof(CharacterStats))
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
        ApplyBuffAndDebuff(caster);
        caster.ReplenishHealt(healAmount + (healPerLevel * level));
    }

    private void ApplyBuffAndDebuff(ActorStats caster)
    {
        BuffDebuffSystem.Debuff debuff;
        BuffDebuffSystem.Buff buff;
        for (int i = 0; i < debuffToAply.Length; i++)
        {            
            debuff = new BuffDebuffSystem.Debuff(debuffToAply[i].type, debuffToAply[i].potency + (debuffPotencyPerLevel[i] * level), debuffToAply[i].remainTime);
            caster.reciveDebuff(debuff);
        }
        for (int i = 0; i < buffToAply.Length; i++)
        {
            buff = new BuffDebuffSystem.Buff(buffToAply[i].type, buffToAply[i].potency + (buffPotencyPerLevel[i] * level), buffToAply[i].remainTime);
            caster.reciveBuff(buff);
        }
    }
}
