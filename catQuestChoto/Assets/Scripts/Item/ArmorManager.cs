using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorManager : MonoBehaviour {

    Armor armorStats;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetStats(Armor stats)
    {
        armorStats = stats;
    }

    public Armor GiveStats()
    {
        return armorStats;
    }
}
