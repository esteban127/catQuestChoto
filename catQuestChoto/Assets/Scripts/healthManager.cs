using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthManager : MonoBehaviour {
    bool alive = true;
    [SerializeField] float HealthPoints;


	public void Death()    
    {
        Debug.Log(gameObject.name + ": Estoy murido");
        alive = false;
    }

    public void getDamage(float damage)
    {
        HealthPoints -= damage;
        if(HealthPoints<= 0)
            alive = false;
    }
	
    public bool isAlive()
    {
        return alive;
    }
}
