using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AtackAbility : IAbility {

    [SerializeField] float abilityDamage = 1; 
    

    public override bool TryCastAbility(GameObject target)
    {
        if(target.tag == "Enemy")
        {
            if ((target.transform.position - this.gameObject.transform.position).magnitude < range && remainCooldown <= 0)
            {
                CastAbility(target.GetComponent<IACTOR>())
                return true;
            }
        }
        return false;
    }

    protected override void CastAbility(IACTOR target)
    {
        throw new NotImplementedException();
    }


}
