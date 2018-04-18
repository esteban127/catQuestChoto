using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthManager : MonoBehaviour {
    public bool alive = true;
	public void Death()
    {
        Debug.Log(gameObject.name + ": Estoy murido");
        alive = false;
    }

    public void getDamage(int damage)
    {
        alive = false; //The damage was to his feelings, he die for loneliness
    }
	
    public bool isAlive()
    {
        return alive;
    }
}
