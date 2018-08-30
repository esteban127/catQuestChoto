using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbility : MonoBehaviour {

    [SerializeField] protected Sprite abilitySprite;
    [SerializeField] protected float range;
    [SerializeField] float baseCooldown;
    protected float cooldown;
    protected float remainCooldown = 0;

    public abstract bool TryCastAbility(GameObject target);
    protected abstract void CastAbility(IACTOR target);

    private void calculateColdown()
    {
        //should calculate final coldown w/ buffs and stuff
        cooldown = baseCooldown;
    }


    private void Start()
    {
        cooldown = baseCooldown;
    }

    private void Update()
    {
        if (remainCooldown > 0)
            remainCooldown -= Time.deltaTime;
    }
}
