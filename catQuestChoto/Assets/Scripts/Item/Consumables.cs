using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumables : Iitem
{
    [SerializeField] BuffDebuffSystem.Buff[] buff;
    [SerializeField] BuffDebuffSystem.Debuff[] debuff;
    public BuffDebuffSystem.Buff[] Buff { get { return buff; } }
    public BuffDebuffSystem.Debuff[] Debuff { get { return debuff; } }

}
