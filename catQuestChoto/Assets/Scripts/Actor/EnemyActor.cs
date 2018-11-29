using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct dropRatio
{
    public int chance;
    public ItemTier tier;
}
[System.Serializable]
public class EnemyActor : IACTOR {

    [SerializeField] dropRatio[] drop;
    public dropRatio[] Drop { get { return drop; } }
    [SerializeField] float baseXpReward;
    public float BaseXpReward { get { return baseXpReward; } }
    [SerializeField] float baseDodgeChance;
    public float BaseDodgeChance { get { return baseDodgeChance; } }
    [SerializeField] float basePrecision;
    public float BasePrecision { get { return basePrecision; } }
    [SerializeField] float critChance;
    public float CritChance { get { return critChance; } }
}
