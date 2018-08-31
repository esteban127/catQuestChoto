using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumables : Iitem
{
    [SerializeField]float duration;
    [SerializeField] bool restoration;

    public void setStats(string name, string c_description, int lvl,bool is_restoration, float c_duration, itemstats aditionalStats)
    {
        itemName = name;      
        itemLvl = lvl;
        restoration = is_restoration;
        description = c_description;
        duration = c_duration;
        stats = aditionalStats;
    }

}
