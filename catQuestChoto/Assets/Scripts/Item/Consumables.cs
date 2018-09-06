using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumables : Iitem
{
    [SerializeField]float duration;
    [SerializeField] bool restoration;

    public void setStats(string name, int lvl,bool is_restoration, float c_duration, itemstats aditionalStats)
    {
        itemName = name;      
        itemLvl = lvl;
        restoration = is_restoration;
        duration = c_duration;
        stats = aditionalStats;
    }

}
