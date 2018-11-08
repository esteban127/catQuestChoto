using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour {
    Iitem itemStats;

    public void SetStats(Iitem stats)
    {
        itemStats = stats;
    }

    public Iitem GiveStats()
    {
        return itemStats;
    }

    public string GetName()
    {
        return itemStats.Name;
    }
}
