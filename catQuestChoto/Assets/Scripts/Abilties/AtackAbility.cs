
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
                        CastAbility(target.GetComponent<ActorStats>(), caster.GetComponent<ActorStats>());
                        return true;
                    }
                }
            }
        }               
        return false;
    }

    private void CastAbility(ActorStats target, ActorStats caster)
    {
        if(caster.GetType() == typeof(CharacterStats))
        {
            if (((CharacterStats)caster).TryToSpendMana(manaCost))
            {
                ResetCooldown();
                if (calculateIfHits(caster, target))
                {                   
                    target.reciveDamage((int)(calculateDamage(caster) * (abilityDamageMultiplier + damagePerLevel*level)));
                }
            }
        }  else
        {
            ResetCooldown();
            if (calculateIfHits(caster, target))
            {    
                target.reciveDamage((int)(calculateDamage(caster) * (abilityDamageMultiplier + +damagePerLevel * level)));
            }
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
            Debug.Log("CRIIIT! " + damage);
        }

        return damage;

    }

    private bool calculateIfHits(ActorStats caster, ActorStats target)
    {
        float roll = Random.Range(0.0f, 1.0f);
        float chanceToDodge = target.DodgeChance() - caster.Precision();        
        if (roll < chanceToDodge)
        {
            Debug.Log("Miss");
            return false;
        }
        else
            return true;
    }
    
}
