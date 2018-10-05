using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    [SerializeField] Weapon weaponStats;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetStats(Weapon stats)
    {
        weaponStats = stats;
    }

    public Weapon GiveStats()
    {
        return weaponStats;
    }

    public string GetName()
    {
        return weaponStats.Name;
    }
}
