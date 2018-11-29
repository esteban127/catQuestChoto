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

        }        
        throw new NotImplementedException();
    }
}
